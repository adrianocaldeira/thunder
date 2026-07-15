# Thunder.NHibernate

O pacote **Thunder.NHibernate** integra o Thunder ao NHibernate: gestão de sessão
(`SessionManager`), um repositório genérico para CRUD e consultas (`Repository<T, TKey>`) e
listeners que carimbam `Created`/`Updated` automaticamente nas entidades.

## Entidade base

Entidades administradas pelo `Repository` herdam de `Persist<T, TKey>` (`Thunder.Data`), que já
implementa `Id`, `Created`, `Updated` e a interface `ICreatedAndUpdatedProperty` usada pelos
listeners de auditoria:

```csharp
using System;
using Thunder.Data;

public class Produto : Persist<Produto, Guid>
{
    public virtual string Nome { get; set; }
    public virtual decimal Preco { get; set; }
}
```

## SessionManager

`SessionManager` mantém uma única `Configuration` e uma única `ISessionFactory` por processo,
inicializadas de forma tardia e thread-safe (`Lazy`). Uma falha na primeira materialização (por
exemplo, banco indisponível na subida da aplicação) é propagada ao chamador, mas não é
permanente — o próximo acesso tenta de novo.

```csharp
using Thunder.NHibernate;

// Sessão corrente: abre e vincula automaticamente se ainda não houver uma vinculada
var session = SessionManager.CurrentSession;
```

Para o padrão *session-per-request* em aplicações web, vincule a sessão no início da
requisição e desvincule ao final:

```csharp
SessionManager.Bind();   // abre e vincula uma nova sessão ao contexto
// ...
SessionManager.Unbind(); // desvincula e descarta a sessão da requisição
```

O pacote já traz filtros de ação prontos que fazem esse ciclo automaticamente por requisição:
`SessionPerRequestAttribute` para ASP.NET MVC e `SessionPerRequestApiAttribute` para Web API
(ambos aceitam `Exclude` com nomes de action separados por vírgula, para pular o bind/unbind
nelas).

```csharp
[SessionPerRequest]
public class ProdutoController : Controller
{
    // cada action (exceto as listadas em Exclude) roda com sessão vinculada
}
```

## Registro dos listeners de auditoria (Created/Updated)

> [!IMPORTANT]
> A partir da 2.x, carimbar `Created`/`Updated` corretamente em entidades mapeadas com
> `dynamic-update="true"` exige registrar **três** listeners, não apenas dois. Sem o
> listener de `FlushEntity`, `Updated` **não é persistido** em updates parciais.

Os listeners são configurados na propriedade estática `SessionManager.Listeners`
(`Dictionary<ListenerType, object[]>`) **antes** do primeiro acesso a
`SessionManager.Configuration`/`SessionFactory` — tipicamente no `Application_Start` do
`Global.asax`. Depois que a configuração é materializada, alterações nessa propriedade não têm
mais efeito.

```csharp
using System.Collections.Generic;
using NHibernate.Event;
using Thunder.NHibernate;
using Thunder.NHibernate.Pattern;

protected void Application_Start()
{
    SessionManager.Listeners = new Dictionary<ListenerType, object[]>
    {
        { ListenerType.PreInsert, new object[] { new CreatedAndUpdatedPropertyEventListener() } },
        { ListenerType.PreUpdate, new object[] { new CreatedAndUpdatedPropertyEventListener() } },
        { ListenerType.FlushEntity, new object[] { new CreatedAndUpdatedFlushEntityListener() } }
    };
}
```

Por que os três:

- **`CreatedAndUpdatedPropertyEventListener`** (`PreInsert`/`PreUpdate`): no insert, grava
  `Created` e `Updated` com o horário local do servidor (sobrescrevendo qualquer valor já
  definido na entidade); no update, grava apenas `Updated` — cobre o insert completo e o
  update completo (reattach sem snapshot carregado).
- **`CreatedAndUpdatedFlushEntityListener`** (`FlushEntity`): cobre o update **parcial** de uma
  entidade *attached* (com snapshot carregado). Com `dynamic-update="true"`, o UPDATE gerado
  contém só as colunas *dirty* — e essas colunas são calculadas **antes** do evento
  `PreUpdate` disparar. Mutar `Updated` só no `PreUpdate` não adianta nesse caminho: a coluna já
  ficou de fora do cálculo de dirty e não entra no SQL. Este listener carimba `Updated` no ponto
  certo do pipeline (antes do dirty-check padrão), garantindo que a coluna seja detectada como
  alterada e entre no UPDATE dinâmico. Ele herda de `DefaultFlushEntityEventListener` e delega ao
  `base` — **não registre também o listener padrão do NHibernate junto**, ou o flush executa
  duas vezes.

Sem a entrada `ListenerType.FlushEntity`, o comportamento antigo é preservado: em entidades com
`dynamic-update="true"`, um update parcial que não altera nenhuma outra coluna simplesmente não
grava `Updated`, porque a coluna nunca aparece no UPDATE dinâmico.

## Repository: CRUD e consultas

`Repository<T, TKey>` (`Thunder.NHibernate.Pattern`) é concreto — não precisa de subclasse — e
cada operação abre sua própria transação:

```csharp
using Thunder.NHibernate.Pattern;

var repository = new Repository<Produto, Guid>();

var produto = repository.Create(new Produto { Nome = "Caneta azul", Preco = 2.50m });

produto = repository.Find(produto.Id);

produto.Preco = 3.00m;
repository.Update(produto);

var existe = repository.Exist(produto.Id, p => p.Nome == "Caneta azul");

repository.Delete(produto.Id);
```

Consultas por expressão, critério (`ICriterion`) ou LINQ (`IQueryable<T>`):

```csharp
using NHibernate.Criterion;

var todos = repository.All();
var comPreco = repository.All(p => p.Preco > 0);
var umProduto = repository.Single(p => p.Nome == "Caneta azul");
var queryable = repository.AllQueryable(p => p.Preco > 0);
```

`UpdateProperty`/`UpdateProperties` atualizam uma ou mais propriedades por identificador sem
precisar carregar e reatribuir a entidade inteira:

```csharp
repository.UpdateProperty(produto.Id, nameof(Produto.Preco), 3.50m);
```

### Paginação

```csharp
var pagina = repository.Page(0, 20); // página 0, 20 itens, ordenado por Id asc
var filtrada = repository.Page(0, 20, p => p.Preco > 0, Order.Desc("Preco"));

pagina.Records;    // total de registros (long)
pagina.PageCount;  // total de páginas
```

### API assíncrona

Todos os métodos síncronos têm equivalente `...Async` com `CancellationToken` opcional; a
transação por método é preservada:

```csharp
var produto = await repository.CreateAsync(new Produto { Nome = "Lápis" });
var lista = await repository.AllAsync();
await repository.DeleteAsync(produto.Id);
```

## ActiveRecord (obsoleto)

`ActiveRecord<T, TKey>` é o antigo padrão estático (`ActiveRecord.Create(...)`,
`ActiveRecord.Find(...)`, etc.). Está marcado `[Obsolete]` a partir da 2.x, delega internamente
para `Repository<T, TKey>` e será removido na 3.0 — para código novo, use `Repository<T, TKey>`
diretamente.
