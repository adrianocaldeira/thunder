# Changelog

Todas as mudanças relevantes dos pacotes Thunder são documentadas neste arquivo.

O formato segue o [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/) e os pacotes
aderem ao [Versionamento Semântico](https://semver.org/lang/pt-BR/). Cada pacote versiona de
forma independente; as seções abaixo agrupam as mudanças por pacote e versão.

Mudanças que alteram comportamento observável são marcadas com **[COMPORTAMENTO]**.

---

## Thunder

### [Não lançado]

#### Corrigido
- **[COMPORTAMENTO]** `IsCpf`: a blacklist de dígitos repetidos continha um valor com 10 dígitos
  (`2222222222`), o que fazia `"22222222222"` (11 dígitos iguais) passar pela validação por falha
  de correspondência na comparação; substituída por verificação direta de dígitos repetidos
  (`cpf.Distinct().Count() == 1`).
- **[COMPORTAMENTO]** `IsZipCode`: lógica de validação estava invertida
  (`!formatoValido || !éBlacklist`), retornando `true` inclusive para entradas sem formato de CEP;
  corrigida para `formatoValido && !éBlacklist`.
- **[COMPORTAMENTO]** `IsPhone`: alternância da regex sem agrupamento
  (`^\((10)|[1-9]{2}\)...`) fazia o DDD `10` contornar o restante da validação; corrigida a
  precedência com agrupamento explícito (`(10|[1-9]{2})`).
- **[COMPORTAMENTO]** `EmailAttribute`: o `IsValid` sobrescrito no server-side só checava a
  existência de exatamente um `@` na string, aceitando `"a@"` e `"@b.com"` como e-mails válidos.
  Removido o override para que a validação use o `RegularExpressionAttribute` base (mesma regra
  do client-side), unificando servidor e cliente numa única fonte de verdade. A regex do
  construtor também tinha um defeito próprio (grupo de domínio exigia dois pontos para casar,
  rejeitando domínios comuns de um único ponto como `"b.com"`) — corrigida a parte de domínio
  para `([\w-]+\.)+\w{2,}$`. String vazia passa a ser aceita pelo atributo (responsabilidade do
  `[Required]`, quando presente).
- **[COMPORTAMENTO]** `IsEmail`: a parte local da regex permitia zero caracteres
  (quantificador `*`), aceitando `"@dominio.com"` como e-mail válido; corrigido para exigir ao
  menos um caractere (`+`).

Impacto no consumidor real (portas-de-entrada): `IsCpf` (9 usos) e `IsPhone` (1 uso) passam a
rejeitar entradas que antes eram aceitas indevidamente — sentido seguro, sem regressão esperada.
`IsZipCode` não é utilizado pelo consumidor. `[Email]` tem 1 uso (`Gateway\Models\Users\Form.cs`,
propriedade `Email`), que também tem `[Required]` — a aceitação de string vazia pelo atributo é
inócua nesse uso. `IsEmail` tem 17 usos no consumidor; a correção só passa a rejeitar entradas
com parte local vazia, que já eram e-mails inválidos.

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
