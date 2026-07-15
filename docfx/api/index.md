# Referência de API

Documentação de referência gerada a partir dos comentários XML dos quatro pacotes do Thunder.
Use a navegação lateral para percorrer os namespaces e tipos, ou a busca no topo da página.

## Pacotes e namespaces

### Thunder
Extensões de tipos, validações e formatações brasileiras, paginação, criptografia e notificações.
- `Thunder` — tipos base (`FormatType`, `Notify`, `Filter`).
- `Thunder.Extensions` — extensões de `string`, `DateTime`, `object` e afins.
- `Thunder.ComponentModel.DataAnnotations` — atributos de validação (ex.: `DocumentAttribute`).
- `Thunder.Collections` — paginação (`Paging`, `IPaging`).
- `Thunder.Security` — criptografia e hash (`AesEncryptor`, `PasswordHasher`).

### Thunder.NHibernate
Padrão Repository/ActiveRecord sobre NHibernate, gestão de sessão e listeners de auditoria.
- `Thunder.NHibernate` — `SessionManager` e infraestrutura de sessão.
- `Thunder.NHibernate.Pattern` — `Repository`, `ActiveRecord` e listeners (`CreatedAndUpdatedFlushEntityListener`).

### Thunder.Web.Mvc
HTML helpers, filtros e model binders para ASP.NET MVC 5.
- `Thunder.Web.Mvc` e subnamespaces — helpers, filtros e binders.

### Thunder.EntityFramework
Padrão Repository sobre Entity Framework 6.
- `Thunder.EntityFramework.Pattern` — `Repository` com unit-of-work e API assíncrona.

> Para exemplos de uso, veja os [Guias](../articles/introducao.md).
