# Paginação e filtros

O pacote **Thunder** traz `Paging<T>` (`Thunder.Collections`), uma lista paginada que também
expõe metadados de paginação, e `Filter` (`Thunder.Model`), uma classe base para filtros de
consulta com paginação e ordenação embutidas.

## Paginando uma coleção

O método de extensão `Paging` (`Thunder.Collections.Extensions`) pagina qualquer
`IEnumerable<T>`/`IQueryable<T>`. `currentPage` é baseado em zero.

```csharp
using Thunder.Collections.Extensions;

IEnumerable<string> produtos = repositorio.Listar(); // 47 itens

var pagina = produtos.Paging(0, 20); // página 0 (primeira), 20 itens por página

pagina.Count;        // 20 (itens desta página — Paging<T> é um List<T>)
pagina.Records;      // 47 (total de itens na fonte)
pagina.PageCount;     // 3  (total de páginas)
pagina.CurrentPage;  // 0
pagina.HasNextPage;   // true
pagina.IsFirstPage;   // true

var ultima = produtos.Paging(2, 20);

ultima.Count;         // 7 (resto da última página)
ultima.IsLastPage;    // true
ultima.HasNextPage;   // false
```

Quando a contagem total já é conhecida (por exemplo, veio de uma consulta separada), passe-a
para evitar um `Count()` adicional na fonte: `produtos.Paging(0, 20, totalDeRegistros)`.

## Filtros de consulta

`Filter` é uma classe base abstrata para modelos de filtro: já vem com `CurrentPage` (inicia
em `0`), `PageSize` (padrão `15`) e uma lista de `Orders` (`FilterOrder`) para ordenação.

```csharp
using Thunder.Model;

public class ProdutoFiltro : Filter
{
    public string Nome { get; set; }
}

var filtro = new ProdutoFiltro
{
    Nome = "Produto",
    CurrentPage = 0,
    PageSize = 20
};

filtro.Orders.Add(new FilterOrder { Column = "Nome", Asc = true });

filtro.Orders[0].ToString(); // "Nome asc"
```

`FilterOrder.Asc` é `true` por padrão; `ToString()` produz `"{Coluna} asc"` ou
`"{Coluna} desc"`, pronto para compor uma cláusula `ORDER BY` dinâmica.
