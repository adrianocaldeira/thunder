# Changelog

Todas as mudanças relevantes dos pacotes Thunder são documentadas neste arquivo.

O formato segue o [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/) e os pacotes
aderem ao [Versionamento Semântico](https://semver.org/lang/pt-BR/). Cada pacote versiona de
forma independente; as seções abaixo agrupam as mudanças por pacote e versão.

Mudanças que alteram comportamento observável são marcadas com **[COMPORTAMENTO]**.

---

## Thunder

### [1.8.1] - 2026-07-14

#### Alterado
- Atualização de dependências (patch, sem mudança de API):
  - log4net 2.0.15 → 2.0.17
  - Newtonsoft.Json 13.0.3 → 13.0.4

### [1.8.0]

- Dependências alinhadas às versões em uso pelos consumidores:
  NHibernate 5.5.2, Newtonsoft.Json 13.0.3, log4net 2.0.15.

---

## Thunder.NHibernate

### [1.2.1] - 2026-07-14

#### Alterado
- Atualização de dependências (patch, sem mudança de API):
  - Newtonsoft.Json 13.0.3 → 13.0.4

### [1.2.0]

- Dependências alinhadas às versões em uso pelos consumidores:
  NHibernate 5.5.2 (corrige CVE-2024-39677), Iesi.Collections 4.1.1,
  Microsoft.AspNet.WebApi 5.3.0, Microsoft.AspNet.WebApi.Client 6.0.0.

---

## Thunder.Web.Mvc

### [1.7.1] - 2026-07-14

#### Alterado
- Atualização de dependências (patch, sem mudança de API):
  - log4net 2.0.15 → 2.0.17
  - Newtonsoft.Json 13.0.3 → 13.0.4

### [1.7.0]

- Dependências alinhadas às versões em uso pelos consumidores:
  Microsoft.AspNet.Mvc 5.3.0, Microsoft.AspNet.Razor 3.3.0, Microsoft.AspNet.WebPages 3.3.0.

---

## Thunder.EntityFramework

### [1.1.1] - 2026-07-14

#### Alterado
- Atualização de dependências (patch, sem mudança de API):
  - EntityFramework 6.5.1 → 6.5.2

### [1.1.0]

- Dependências alinhadas às versões em uso pelos consumidores: EntityFramework 6.5.1.
