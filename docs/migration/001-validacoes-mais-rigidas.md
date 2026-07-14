# 001 — Validações mais rígidas e correções de comportamento

**Pacotes afetados:** Thunder 1.9.0 · Thunder.Web.Mvc 1.8.0
**Tipo de versão:** minor (comportamento mais correto, compatível com o uso esperado da API)

## O que mudou

Uma revisão de segurança e correção de bugs identificou várias validações e utilitários que
aceitavam entradas malformadas ou se comportavam de forma diferente do documentado. As APIs
públicas continuam com a mesma assinatura; o que muda é o resultado retornado para entradas que
antes passavam indevidamente.

### `IsCpf` (Thunder)

- **Antes**: a lista de CPFs com dígitos repetidos rejeitados continha um valor com 10 dígitos em
  vez de 11, então `"22222222222"` (11 dígitos iguais) passava como CPF válido.
- **Depois**: qualquer sequência de dígitos repetidos é rejeitada, verificada diretamente
  (independente de blacklist).

### `IsZipCode` (Thunder)

- **Antes**: a condição de validação estava invertida e retornava `true` inclusive para entradas
  sem formato de CEP.
- **Depois**: só retorna `true` para entradas que casam o formato de CEP e não estão na
  blacklist.

### `IsPhone` (Thunder)

- **Antes**: a regex usava alternância sem agrupamento em torno do DDD, o que permitia que o DDD
  `10` contornasse o restante da validação.
- **Depois**: o agrupamento foi corrigido e o DDD `10` passa pelas mesmas regras que os demais.

### `EmailAttribute` / atributo `[Email]` (Thunder)

- **Antes**: o `IsValid` do lado servidor checava apenas a existência de um único `@` na string,
  aceitando valores como `"a@"` ou `"@b.com"`. A regex do lado cliente, por sua vez, exigia que o
  domínio tivesse pelo menos dois pontos, rejeitando domínios válidos de um único ponto como
  `"b.com"`.
- **Depois**: o servidor usa a mesma regra de validação do cliente (uma única fonte de verdade), e
  a regex de domínio foi corrigida para aceitar domínios de um ponto. String vazia passa a ser
  aceita pelo atributo — quem depende de campo obrigatório deve combinar com `[Required]`.

### `IsEmail` (Thunder)

- **Antes**: a parte local do e-mail podia ter zero caracteres, então `"@dominio.com"` era aceito
  como e-mail válido.
- **Depois**: a parte local exige ao menos um caractere.

### `ObjectExtensions.Cast<T>` (Thunder)

- **Antes**: a conversão usava a cultura corrente da thread. Em uma thread com cultura pt-BR,
  `"1,5".Cast<decimal>()` retornava `1.5m` (vírgula como separador decimal).
- **Depois**: a conversão usa `CultureInfo.InvariantCulture` de forma determinística,
  independentemente da cultura da thread.
- **Efeito colateral importante**: no invariant, vírgula é separador de milhar, não separador
  decimal. Isso significa que `"1,5".Cast<decimal>()` passa a retornar `15m` (interpretando a
  vírgula como separador de milhar), o oposto do valor anterior em pt-BR. Strings com ponto como
  separador decimal (`"1.5"`) continuam retornando `1.5m` normalmente.

### `Controller.Success(data, contentType)` (Thunder.Web.Mvc)

- **Antes**: o parâmetro `contentType` era ignorado; a resposta saía sempre com
  `"application/json"`.
- **Depois**: o `contentType` informado é respeitado.

### `ModelStateDictionaryExtensions.ExcludePropertiesWithKeyPart` (Thunder.Web.Mvc)

- **Antes**: o predicado estava invertido, tornando o método um no-op sempre que `ignoreKeys` era
  `null` (o caso mais comum, sem lista de exceções).
- **Depois**: o método efetivamente exclui as propriedades esperadas mesmo sem lista de exceções.

### `CompressAttribute` (Thunder.Web.Mvc)

- **Antes**: o cabeçalho `Vary` era emitido com o valor `Content-Encoding`, sem efeito prático
  para caches/proxies.
- **Depois**: o cabeçalho `Vary` referencia `Accept-Encoding`, valor correto para indicar que a
  resposta varia conforme a codificação aceita pelo cliente. A compressão em si não muda.

### `NotifyResult` — renderização HTML (Thunder.Web.Mvc)

- **Antes**: no caminho de renderização HTML (não-Ajax), as mensagens eram impressas via
  `KeyValuePair.ToString()` em vez do texto real da mensagem, e sem qualquer encoding.
- **Depois**: o texto de cada mensagem é extraído corretamente e passa por
  `HttpUtility.HtmlEncode` antes de ser inserido na saída HTML.

## Por que mudou

Todas as correções acima partiram de bugs objetivos identificados em auditoria: lógica invertida,
regex incorreta, blacklist incompleta, parâmetro ignorado ou ausência de encoding. Não são
mudanças de critério ou de política de validação — são correções para que o comportamento
corresponda ao que a documentação e a assinatura da API sempre indicaram.

## Impacto esperado

- **Validações ficam mais rígidas**: `IsCpf`, `IsZipCode`, `IsPhone`, `IsEmail` e `[Email]` passam
  a rejeitar entradas malformadas que antes eram aceitas indevidamente. Qualquer fluxo que
  dependesse (mesmo sem perceber) de aceitar essas entradas malformadas passa a receber `false`/erro
  de validação onde antes recebia sucesso.
- **`Cast<T>` com vírgula decimal muda de resultado**: qualquer código que chamava
  `Cast<decimal>()`/`Cast<double>()` com strings no formato pt-BR (vírgula como separador decimal,
  ex. `"1,5"`) passa a obter um valor numérico diferente (a vírgula agora é lida como separador de
  milhar). Isso não gera exceção — o risco é um valor numericamente errado passar despercebido.
- **`[Email]` aceita string vazia**: formulários que dependiam do atributo para rejeitar campo de
  e-mail vazio precisam garantir `[Required]` explícito na mesma propriedade.
- **`Success(data, contentType)`**: chamadas que informavam um `contentType` diferente de
  `application/json` esperando que fosse ignorado (e a resposta saísse como JSON mesmo assim)
  passam a receber o `contentType` pedido.
- **`NotifyResult` em HTML**: código que dependia de injetar marcação HTML dentro das mensagens de
  notificação (para formatação, links, etc.) no caminho não-Ajax passa a ver essa marcação
  encodada como texto literal na tela.

## Passo a passo de migração

1. **Audite chamadas de validação** (`IsCpf`, `IsZipCode`, `IsPhone`, `IsEmail`, atributo
   `[Email]`) na aplicação consumidora. Rode a suíte de testes de formulários/cadastros e observe
   se algum CPF, CEP, telefone ou e-mail que antes passava na validação agora é rejeitado — nesse
   caso, o dado de entrada provavelmente sempre foi inválido e o formulário deve tratar o erro,
   não contornar a validação.
2. **Teste especificamente o atributo `[Email]` no client-side** (validação jQuery Unobtrusive):
   como a regex de domínio mudou, confirme que domínios de um único ponto (ex. `"nome@empresa.com"`)
   continuam validando e que a mensagem de erro aparece corretamente para entradas inválidas.
   Se algum campo depende de `[Email]` para rejeitar vazio, adicione `[Required]`.
3. **Audite usos de `Cast<decimal>()`/`Cast<double>()`** com strings que possam conter vírgula
   decimal (formulários, importação de planilha, integração com sistemas em pt-BR). Normalize a
   string para o formato invariant (ponto como separador decimal) antes de chamar `Cast<T>`, ou
   troque por `decimal.Parse`/`double.Parse` com `CultureInfo` explícita quando o formato de
   entrada for conhecido.
4. **Revise chamadas a `Success(data, contentType)`** que informam um `contentType` diferente do
   padrão — confirme se o valor agora realmente aplicado é o esperado pelo cliente que consome a
   resposta.
5. **Confira consumidores de `NotifyResult` no caminho HTML (não-Ajax)** que montavam mensagens
   contendo marcação HTML esperando que fosse renderizada — após a atualização, esse HTML aparece
   encodado (texto literal). Ajuste as mensagens para texto puro, ou trate a formatação por outro
   meio (ex. view/partial dedicada) fora do texto da mensagem.

## Referências

Detalhes completos de cada correção, incluindo trechos de código antes/depois quando aplicável,
estão nas entradas marcadas com **[COMPORTAMENTO]** no [CHANGELOG](../../CHANGELOG.md), seções
Thunder 1.9.0 e Thunder.Web.Mvc 1.8.0.
