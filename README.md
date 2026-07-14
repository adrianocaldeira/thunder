# Thunder

[English version](README.en.md)

[![Licença: MIT](https://img.shields.io/badge/licença-MIT-blue.svg)](license.txt)

Conjunto de bibliotecas utilitárias para **.NET Framework 4.8**: extensões de tipos, validações
brasileiras (CPF/CNPJ/e-mail/CEP/telefone), padrões de acesso a dados sobre NHibernate e Entity
Framework, e HTML helpers para ASP.NET MVC 5.

## Pacotes

| Pacote | Versão | Downloads | Descrição |
|---|---|---|---|
| [Thunder](https://www.nuget.org/packages/Thunder/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.svg)](https://www.nuget.org/packages/Thunder/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.svg)](https://www.nuget.org/packages/Thunder/) | Extensões de tipos, validações brasileiras (CPF/CNPJ/e-mail/CEP/telefone), formatação, paginação, criptografia/hash e notificações. |
| [Thunder.NHibernate](https://www.nuget.org/packages/Thunder.NHibernate/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.NHibernate.svg)](https://www.nuget.org/packages/Thunder.NHibernate/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.NHibernate.svg)](https://www.nuget.org/packages/Thunder.NHibernate/) | Padrão Repository/ActiveRecord sobre NHibernate 5.5+, gestão de sessão e listeners de auditoria (Created/Updated). |
| [Thunder.Web.Mvc](https://www.nuget.org/packages/Thunder.Web.Mvc/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.Web.Mvc.svg)](https://www.nuget.org/packages/Thunder.Web.Mvc/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.Web.Mvc.svg)](https://www.nuget.org/packages/Thunder.Web.Mvc/) | HTML helpers, filtros e model binders para ASP.NET MVC 5. |
| [Thunder.EntityFramework](https://www.nuget.org/packages/Thunder.EntityFramework/) | [![NuGet](https://img.shields.io/nuget/v/Thunder.EntityFramework.svg)](https://www.nuget.org/packages/Thunder.EntityFramework/) | [![Downloads](https://img.shields.io/nuget/dt/Thunder.EntityFramework.svg)](https://www.nuget.org/packages/Thunder.EntityFramework/) | Padrão Repository sobre Entity Framework 6. |

## Instalação

Os pacotes usam o formato clássico `packages.config`. A forma recomendada de instalar é pelo
**Package Manager Console** do Visual Studio:

```powershell
Install-Package Thunder
Install-Package Thunder.NHibernate
Install-Package Thunder.Web.Mvc
Install-Package Thunder.EntityFramework
```

Para restaurar os pacotes de uma solution já configurada (build local ou CI), use o `nuget.exe`:

```
nuget restore Thunder.sln
```

## Quickstart

```csharp
using Thunder;
using Thunder.Extensions;
using Thunder.Collections.Extensions;

// CPF/CNPJ: validar e formatar
var cpf = "53778321676";

if (cpf.IsCpf())
    Console.WriteLine(cpf.Format(FormatType.Cpf)); // 537.783.216-76

// Paginação: página 0 (primeira), 20 itens por página
IEnumerable<Produto> produtos = repositorio.Listar();
var pagina = produtos.Paging(0, 20);
```

### Thunder.NHibernate: listener de auditoria

Para preencher automaticamente as propriedades de criação/atualização (`Created`/`Updated`),
registre o listener na configuração da `session-factory`:

```xml
<session-factory>
    ...
    <listener type="pre-update" class="Thunder.NHibernate.Pattern.CreatedAndUpdatedPropertyEventListener, Thunder.NHibernate"/>
    <listener type="pre-insert" class="Thunder.NHibernate.Pattern.CreatedAndUpdatedPropertyEventListener, Thunder.NHibernate"/>
</session-factory>
```

## Frameworks suportados

| Pacote | .NET Framework |
|---|---|
| Thunder | 4.8 |
| Thunder.NHibernate | 4.8 |
| Thunder.Web.Mvc | 4.8 |
| Thunder.EntityFramework | 4.8 |

## Mais informações

- [CHANGELOG](CHANGELOG.md)
- [Guias de migração](docs/migration/)

## Licença

Distribuído sob a licença MIT. Veja [license.txt](license.txt).
