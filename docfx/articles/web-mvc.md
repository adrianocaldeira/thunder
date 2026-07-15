# Thunder.Web.Mvc

O pacote **Thunder.Web.Mvc** traz HTML helpers, model binders, filtros de ação e utilitários de
URL/JSON para **ASP.NET MVC 5**.

> [!NOTE]
> Parte dos helpers (`Html/Design/SimplaAdmin/*`) foi construída para o tema visual
> **SimplaAdmin** e é acoplada ao HTML/CSS/JavaScript desse tema (classes CSS, estrutura de
> markup específica). São úteis apenas em projetos que já usam o SimplaAdmin; não são um
> componente visual genérico. Os demais helpers (`Html.Thunder()`, model binders, filtros,
> `Controller`, `UrlHelperExtensions`) não têm essa dependência.

## HTML helpers (`Html.Thunder()`)

O ponto de entrada é o método de extensão `Thunder()` sobre `HtmlHelper<TModel>`, que expõe um
conjunto de `...For` análogos ao `TextBoxFor` padrão do MVC, cada um aplicando a máscara/classe
CSS correspondente (`cpf`, `cnpj`, `currency` etc. — o comportamento de máscara em si é
JavaScript de responsabilidade da view/tema, os helpers só emitem o `<input>` com os atributos
certos):

```csharp
@using Thunder.Web.Mvc.Html
@model ProdutoViewModel

@Html.Thunder().TextBoxFor(m => m.Nome)
@Html.Thunder().CpfFor(m => m.Cpf)
@Html.Thunder().CnpjFor(m => m.Cnpj)
@Html.Thunder().CurrencyFor(m => m.Preco)
@Html.Thunder().DateFor(m => m.DataNascimento)
@Html.Thunder().PhoneFor(m => m.Telefone)
@Html.Thunder().ZipCodeFor(m => m.Cep)
@Html.Thunder().NumericFor(m => m.Quantidade)
@Html.Thunder().PasswordFor(m => m.Senha)
@Html.Thunder().TextAreaFor(m => m.Observacoes)
```

Também há helpers para paginação e notificações, reaproveitando `IPaging<T>` e `Notify`
(`Thunder`, pacote core):

```csharp
@Html.Thunder().Pagination(Model.Produtos, pagina => Url.Action("Index", new { pagina }))
@Html.Thunder().Notify(showCloseButton: true)
```

## SelectListExtensions

Métodos de extensão para converter listas em `SelectListItem`, incluindo um atalho para enums
que usa `[Display]` (com fallback para o nome do membro, via `EnumExtensions.DisplayName()`):

```csharp
using Thunder.Web.Mvc;

IList<Produto> produtos = repository.All();

var itens = produtos.ToSelectList(p => p.Nome, p => p.Id.ToString());

var selectList = SelectListExtensions.ToSelectList<StatusPedido>(); // enum -> SelectList
```

## Model binders

`DecimalModelBinder` interpreta o valor na cultura corrente da requisição (mantendo o formato
pt-BR quando a aplicação roda em `pt-BR`), retorna `null` com segurança quando o campo está
ausente e adiciona um erro de model state amigável em vez de deixar `FormatException`/
`OverflowException` propagar para valores inválidos ou fora da faixa de `decimal`:

```csharp
// Global.asax / Application_Start
using System.Web.Mvc;
using Thunder.Web.Mvc.Binders;

ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
```

## Filtros de ação

| Filtro | Uso |
|---|---|
| `CompressAttribute` | Comprime a resposta com gzip/deflate conforme o `Accept-Encoding` da requisição; define `Vary: Accept-Encoding`. |
| `ClearCacheAttribute` | Desabilita cache HTTP da resposta (`no-cache`, `no-store`). |
| `RedirectToCanonicalUrlAttribute` | Redireciona (301) URLs não canônicas — barra final e/ou caixa — para a forma canônica. Combine com `NoTrailingSlashAttribute`/`NoLowercaseQueryStringAttribute` para excluir uma action específica dessas regras. |
| `LayoutInjectAttribute` | Define a `_Layout`/master view do `ViewResult` da action, exceto em requisições Ajax ou views parciais (nome iniciando com `_`). |

```csharp
[Compress]
[ClearCache]
[RedirectToCanonicalUrl(appendTrailingSlash: false, lowercaseUrls: true)]
public class ProdutoController : Controller
{
    // ...
}
```

## Controller

`Thunder.Web.Mvc.Controller` estende o `Controller` padrão do MVC com atalhos para notificação
via `TempData`/`Session` e respostas JSON/`NotifyResult` padronizadas:

```csharp
using Thunder.Web.Mvc;

public class ProdutoController : Controller
{
    public ActionResult Salvar(ProdutoViewModel model)
    {
        if (!ModelState.IsValid)
            return Notify(NotifyType.Danger, ModelState);

        // ... persistir produto ...

        SetNotify(new Notify("Produto salvo com sucesso."));
        return RedirectToAction("Index");
    }

    public JsonResult Listar()
    {
        return Success(repository.All());
    }
}
```

`NotifyResult`/`JsonResult` (as versões do Thunder, não as do MVC) tratam o parâmetro `callback`
da querystring para respostas JSONP: o nome do callback é validado contra um padrão restrito de
identificador JavaScript antes de ser ecoado na resposta; um callback inválido faz a resposta
permanecer como JSON puro, sem o wrapper JSONP.

## UrlHelperExtensions — URLs absolutas

`AbsoluteAction`, `AbsoluteRouteUrl` e `AbsoluteContent` geram URLs absolutas a partir de uma
action, rota ou caminho de conteúdo:

```csharp
using Thunder.Web.Mvc.Extensions;

var url = Url.AbsoluteAction("Detalhe", "Produto", new { id = produto.Id });
```

Por padrão, a autoridade (host) vem da própria requisição recebida. Para ambientes atrás de
proxy/CDN onde o cabeçalho `Host` não é confiável, configure um host canônico via AppSetting —
as URLs absolutas passam a usar sempre esse valor, independentemente do `Host` da requisição:

```xml
<add key="Thunder.Web.Mvc.CanonicalHost" value="www.meusite.com.br" />
```

## Tema SimplaAdmin (legado)

Os extension methods em `Thunder.Web.Mvc.Html.Design.SimplaAdmin` (`ContentBoxExtensions`,
`ButtonExtensions`, `LabelExtensions`, `PaginationExtensions`, `MessageExtensions`) só fazem
sentido em conjunto com o markup/CSS/JS do tema SimplaAdmin — são helpers de apresentação para
esse tema específico, não um design system do Thunder. `MessageExtensions` (`Html.Success()`,
`Html.Attention()`, `Html.Information()`, `Html.Error()`) aplica encode HTML em toda mensagem
antes de renderizar:

```csharp
@Html.Success("Registro salvo com sucesso.")
@Html.Error(new List<string> { "CPF inválido.", "Campo obrigatório." })
```
