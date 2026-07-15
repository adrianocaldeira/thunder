# Introdução

O **Thunder** é um conjunto de bibliotecas utilitárias para **.NET Framework 4.8**, distribuído
em quatro pacotes NuGet independentes. O pacote core (**Thunder**) reúne extensões de tipos,
validações e formatações brasileiras (CPF, CNPJ — inclusive alfanumérico —, e-mail, CEP,
telefone), paginação, criptografia/hash e notificações; os demais pacotes agregam integrações
com NHibernate, ASP.NET MVC 5 e Entity Framework 6.

## Pacotes

| Pacote | Descrição |
|---|---|
| **Thunder** | Extensões de tipos, validações/formatações BR, paginação, criptografia/hash, notificações. |
| **Thunder.NHibernate** | `ActiveRecord`/`Repository` e gestão de sessão sobre NHibernate. |
| **Thunder.Web.Mvc** | HTML helpers, filtros e model binders para ASP.NET MVC 5. |
| **Thunder.EntityFramework** | `Repository` sobre Entity Framework 6. |

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
nuget restore MinhaSolution.sln
```

## Frameworks suportados

Todos os pacotes têm como alvo mínimo o **.NET Framework 4.8**.

## Quickstart

```csharp
using System.Collections.Generic;
using Thunder;
using Thunder.Extensions;
using Thunder.Collections.Extensions;

// CPF/CNPJ: validar e formatar
var cpf = "53778321676";

if (cpf.IsCpf())
    Console.WriteLine(cpf.Format(FormatType.Cpf)); // 537.783.216-76

var cnpj = "11222333000181";

if (cnpj.IsCnpj())
    Console.WriteLine(cnpj.Format(FormatType.Cnpj)); // 11.222.333/0001-81

// Paginação: página 0 (primeira), 20 itens por página
IEnumerable<string> produtos = repositorio.Listar();
var pagina = produtos.Paging(0, 20);
```

A partir daqui, cada guia cobre um recurso do pacote core em detalhe: [validações e
formatações brasileiras](validacoes-br.md), [extensões de tipos](extensoes.md),
[paginação e filtros](paginacao-filtros.md), [criptografia e hash](criptografia.md) e
[notificações](notificacoes.md).
