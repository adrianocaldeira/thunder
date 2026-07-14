# Thunder

[Versão em português](README.md)

[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](license.txt)

A set of utility libraries for **.NET Framework 4.8**: type extensions, Brazilian document/format
validations (CPF/CNPJ/e-mail/ZIP code/phone), data access patterns on top of NHibernate and Entity
Framework, and HTML helpers for ASP.NET MVC 5.

## Packages

| Package | Version | Downloads | Description |
|---|---|---|---|
| [Thunder](https://www.nuget.org/packages/Thunder/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.svg)](https://www.nuget.org/packages/Thunder/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.svg)](https://www.nuget.org/packages/Thunder/) | Type extensions, Brazilian validations (CPF/CNPJ/e-mail/ZIP code/phone), formatting, paging, cryptography/hashing and notifications. |
| [Thunder.NHibernate](https://www.nuget.org/packages/Thunder.NHibernate/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.NHibernate.svg)](https://www.nuget.org/packages/Thunder.NHibernate/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.NHibernate.svg)](https://www.nuget.org/packages/Thunder.NHibernate/) | Repository/ActiveRecord pattern on top of NHibernate 5.5+, session management and audit listeners (Created/Updated). |
| [Thunder.Web.Mvc](https://www.nuget.org/packages/Thunder.Web.Mvc/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.Web.Mvc.svg)](https://www.nuget.org/packages/Thunder.Web.Mvc/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.Web.Mvc.svg)](https://www.nuget.org/packages/Thunder.Web.Mvc/) | HTML helpers, filters and model binders for ASP.NET MVC 5. |
| [Thunder.EntityFramework](https://www.nuget.org/packages/Thunder.EntityFramework/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.EntityFramework.svg)](https://www.nuget.org/packages/Thunder.EntityFramework/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.EntityFramework.svg)](https://www.nuget.org/packages/Thunder.EntityFramework/) | Repository pattern on top of Entity Framework 6. |

## Installation

Packages use the classic `packages.config` format. The recommended way to install them is through
Visual Studio's **Package Manager Console**:

```powershell
Install-Package Thunder
Install-Package Thunder.NHibernate
Install-Package Thunder.Web.Mvc
Install-Package Thunder.EntityFramework
```

To restore packages for an already configured solution (local build or CI), use `nuget.exe`:

```
nuget restore Thunder.sln
```

## Quickstart

```csharp
using Thunder;
using Thunder.Extensions;
using Thunder.Collections.Extensions;

// CPF/CNPJ: validate and format
var cpf = "53778321676";

if (cpf.IsCpf())
    Console.WriteLine(cpf.Format(FormatType.Cpf)); // 537.783.216-76

// Paging: page 0 (first page), 20 items per page
IEnumerable<Produto> produtos = repositorio.Listar();
var pagina = produtos.Paging(0, 20);
```

### Thunder.NHibernate: audit listener

To automatically fill the `Created`/`Updated` audit properties, register the listener in your
`session-factory` configuration:

```xml
<session-factory>
    ...
    <listener type="pre-update" class="Thunder.NHibernate.Pattern.CreatedAndUpdatedPropertyEventListener, Thunder.NHibernate"/>
    <listener type="pre-insert" class="Thunder.NHibernate.Pattern.CreatedAndUpdatedPropertyEventListener, Thunder.NHibernate"/>
</session-factory>
```

## Supported frameworks

| Package | .NET Framework |
|---|---|
| Thunder | 4.8 |
| Thunder.NHibernate | 4.8 |
| Thunder.Web.Mvc | 4.8 |
| Thunder.EntityFramework | 4.8 |

## More information

- [CHANGELOG](CHANGELOG.md)
- [Migration guides](docs/migration/)

## License

Distributed under the MIT license. See [license.txt](license.txt).
