# Thunder.EntityFramework

O pacote **Thunder.EntityFramework** traz um repositório genérico (`Repository<T, TKey>`) sobre
**Entity Framework 6**, com padrão *unit-of-work* explícito.

## Entidade base

Assim como no Thunder.NHibernate, as entidades herdam de `Persist<T, TKey>` (`Thunder.Data`):

```csharp
using System;
using Thunder.Data;

public class Produto : Persist<Produto, Guid>
{
    public virtual string Nome { get; set; }
    public virtual decimal Preco { get; set; }
}
```

## Repository

`Repository<T, TKey>` (`Thunder.EntityFramework.Pattern`) é abstrato e recebe o `DbContext` no
construtor — o repositório **não** é dono do contexto: quem o cria (tipicamente o container de
injeção de dependência) é responsável pelo seu ciclo de vida, e o repositório não implementa
`IDisposable`.

```csharp
using Thunder.EntityFramework.Pattern;

public class ProdutoRepository : Repository<Produto, Guid>
{
    public ProdutoRepository(MeuDbContext context) : base(context)
    {
    }
}
```

### Unit-of-work: Add/Update/Delete + Save

`Add`, `Update` e `Delete` apenas alteram o estado das entidades rastreadas pelo contexto — a
persistência só ocorre ao chamar `Save()`/`SaveAsync()`:

```csharp
using (var context = new MeuDbContext())
{
    var repository = new ProdutoRepository(context);

    var produto = new Produto { Nome = "Caneta azul", Preco = 2.50m };

    repository.Add(produto);
    repository.Save(); // só agora o INSERT é executado

    produto.Preco = 3.00m;
    repository.Update(produto);
    repository.Save(); // UPDATE

    repository.Delete(produto.Id);
    repository.Save(); // DELETE
}
```

Várias mutações podem ser acumuladas e persistidas numa única chamada a `Save`:

```csharp
repository.Add(new Produto { Nome = "Lápis", Preco = 1.20m });
repository.Add(new Produto { Nome = "Borracha", Preco = 0.90m });
repository.Delete(produtoAntigo.Id);

repository.Save(); // um único SaveChanges para as três mutações
```

### Leituras: All/FindBy (AsNoTracking) e Single (rastreado)

`All()` e `FindBy(predicate)` retornam `IQueryable<T>` **sem rastreamento**
(`AsNoTracking()`) — adequado para consultas somente leitura, sem o custo do change tracker.
Para atualizar uma entidade obtida assim, chame `Update` explicitamente antes de `Save`.
`Single(id)` busca pela chave e retorna a entidade **rastreada**:

```csharp
var todos = repository.All().ToList();
var caros = repository.FindBy(p => p.Preco > 10m).ToList();

var produto = repository.Single(id); // rastreado — Update funciona sem chamada extra
```

### API assíncrona

```csharp
var produto = await repository.SingleAsync(id);

repository.Add(new Produto { Nome = "Marcador" });
await repository.SaveAsync();
```
