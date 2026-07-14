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
- `BooleanExtensions.Text()`: o literal `"N�o"` estava com a codificação corrompida no fonte
  (mojibake); regravado como `"Não"` (arquivo salvo em UTF-8 com BOM, padrão do repositório).
- `EnumExtensions.DisplayName()`: lançava `IndexOutOfRangeException` para enums sem
  `[Display]`; adicionado fallback para o nome do membro (`Enum.GetName`).
- **[COMPORTAMENTO]** `ObjectExtensions.Cast<T>`: os `Convert.ChangeType` usavam a cultura
  corrente da thread, fazendo `"1.5".Cast<decimal>()` virar `15m` em pt-BR; passaram a usar
  `CultureInfo.InvariantCulture` de forma determinística. Efeito colateral: strings com vírgula
  decimal (`"1,5"`) agora são interpretadas com a vírgula como separador de milhar do invariant
  (também viram `15m`), independente da cultura da thread — comportamento estável, porém oposto
  ao de antes para esse caso específico. Sem uso confirmado de `Cast<decimal/double>` com
  strings pt-BR no consumidor real (portas-de-entrada).

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

### [Não lançado]

#### Corrigido
- `SelectListExtensions.ToSelectList<T>()`: lançava `IndexOutOfRangeException` para enums sem
  `[Display]`; passou a usar `EnumExtensions.DisplayName()`, com fallback para o nome do membro.
- **[COMPORTAMENTO]** `Controller.Success(data, contentType)`: o parâmetro `contentType` era
  ignorado e o overload sempre sobrescrevia para `"application/json"`; agora o valor informado é
  respeitado. Sem uso confirmado no consumidor real (portas-de-entrada).
- **[COMPORTAMENTO]** `ModelStateDictionaryExtensions.ExcludePropertiesWithKeyPart`: o predicado
  estava invertido (`ignoreKeys != null && !ignoreKeys.Contains(...)`), tornando o método um
  no-op sempre que `ignoreKeys` era `null` — o caso mais comum, sem lista de exceções; corrigido
  para `ignoreKeys == null || !ignoreKeys.Contains(...)`. Sem uso confirmado no consumidor real.
- **[COMPORTAMENTO]** `CompressAttribute`: o cabeçalho `Vary` era emitido com o valor
  `Content-Encoding` (cabeçalho de resposta, sem efeito para caches/proxies), quando deveria
  referenciar `Accept-Encoding` (cabeçalho de requisição que de fato varia a resposta gzip);
  corrigido. Usado pelo consumidor real via registro global do filtro no Gateway — mudança
  segura, apenas corrige um cabeçalho incorreto, sem alterar a compressão em si.
- `NotifyResult` (renderização HTML, caminho não-Ajax): `Messages` é uma
  `IList<KeyValuePair<string, IList<string>>>`; o caminho HTML imprimia
  `KeyValuePair.ToString()` em vez do texto da mensagem. Corrigido para extrair as mensagens de
  cada `Value` e aplicar `HttpUtility.HtmlEncode` em cada uma antes de montar o
  `<li>`/corpo — mitigação pontual de XSS neste caminho específico; o encode abrangente de toda
  a superfície de mensagens (A2 da auditoria) fica para a Fase 2.

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
