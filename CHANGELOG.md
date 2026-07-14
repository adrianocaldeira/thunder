# Changelog

Todas as mudanças relevantes dos pacotes Thunder são documentadas neste arquivo.

O formato segue o [Keep a Changelog](https://keepachangelog.com/pt-BR/1.1.0/) e os pacotes
aderem ao [Versionamento Semântico](https://semver.org/lang/pt-BR/). Cada pacote versiona de
forma independente; as seções abaixo agrupam as mudanças por pacote e versão.

Mudanças que alteram comportamento observável são marcadas com **[COMPORTAMENTO]**.

---

## Thunder

### [2.0.0] - 2026-07-14

Mudanças de comportamento e remoções detalhadas no
[guia de migração 004](docs/migration/004-thunder-2.0.md).

#### Removido
- **`Hash`, `HashHelper`, `HashProvider`** (descontinuados na 1.10.0): removidos. Substitutos —
  `PasswordHasher.Hash`/`Verify` para hash de senha; `AesEncryptor.Encrypt`/`Decrypt` para
  cifragem reversível. Não há substituto que reproduza o formato de hash legado; a migração de
  dados existentes é descrita no [guia 003](docs/migration/003-criptografia-v2.md).

#### Corrigido
- **[COMPORTAMENTO]** Identidade de entidade em `Persist<T, TKey>`:
  - `IsNew()` passa a funcionar com qualquer tipo de chave (`Guid`, `string`, numérico), não só
    numérico. Antes, uma entidade era considerada "nova" quando `Id <= 0` — regra válida apenas
    para chaves numéricas; agora é considerada nova apenas quando `Id` é igual ao valor padrão do
    tipo (`default(TKey)`). **Consequência:** um `Id` numérico negativo deixa de ser tratado como
    "novo".
  - `Equals` passa a comparar `Id` **e** o tipo da entidade (antes comparava o hash code, o que
    podia dar falsos positivos/negativos).
  - `GetHashCode` é estável e não lança exceção quando `Id` é nulo. Uma entidade transiente
    colocada numa coleção baseada em hash (`HashSet`/`Dictionary`) antes de persistir mantém o
    hash de identidade de referência (relevante em cenários detached/multi-sessão).

#### Alterado
- Build convertido para SDK-style. Sem impacto de API ou de comportamento em runtime.

### [1.10.0] - 2026-07-14

#### Adicionado
- `Thunder.Security.AesEncryptor`: nova API estática de criptografia simétrica autenticada.
  `Encrypt`/`Decrypt` usam AES-256 em modo CBC com IV e salt aleatórios gerados a cada chamada,
  chave derivada por PBKDF2-SHA256 (600.000 iterações) e autenticação Encrypt-then-MAC via
  HMAC-SHA256 sobre versão, salt, IV e texto cifrado. A verificação do MAC (em tempo constante)
  acontece antes de qualquer tentativa de descriptografia — evita ataques de padding oracle e
  garante que uma senha errada ou payload adulterado sempre lance `CryptographicException`, nunca
  retorne texto corrompido silenciosamente.
- `Thunder.Security.PasswordHasher`: nova API estática para hash de senha. `Hash` usa
  PBKDF2-SHA256 com salt aleatório de 16 bytes e 600.000 iterações, produzindo uma string
  autodescritiva (`"{iterações}.{salt em Base64}.{subchave em Base64}"`) que carrega os próprios
  parâmetros usados. `Verify` rederiva a subchave com o mesmo salt/iterações do hash informado e
  compara em tempo constante; por segurança, só aceita hashes cuja contagem de iterações esteja
  entre 100.000 e 2.000.000 — fora dessa faixa o hash é rejeitado antes de qualquer derivação de
  chave (proteção contra negação de serviço via hash forjado com iteração absurda). Nunca lança
  exceção para senha errada ou hash malformado: apenas retorna `false`.

#### Descontinuado
- **`Cryptography`, `CryptographyHelper.Encrypt`/`Decrypt`**: marcados `[Obsolete]`. A
  implementação usa IV fixo (o mesmo IV para toda cifragem, independentemente da chave) e
  derivação de chave sem controle de iterações — vulnerável a análise de padrão entre mensagens e
  a ataques de força bruta/dicionário contra a chave. Substituto: `AesEncryptor.Encrypt`/`Decrypt`.
  Continuam funcionando, para permitir a leitura de dados já cifrados pela API legada durante a
  migração.
- **`Hash`, `HashHelper.Hash`**: marcados `[Obsolete]`. O provedor padrão é SHA-1 sem salt,
  inadequado para senhas. Substituto: `PasswordHasher.Hash`/`Verify` para senhas; para hash sem
  finalidade de senha, usar um algoritmo SHA-256 ou superior explicitamente.
- Passo a passo de transição e aviso sobre incompatibilidade de dados entre as duas APIs no
  [guia de migração 003](docs/migration/003-criptografia-v2.md).

### [1.9.0] - 2026-07-14

Mudanças de comportamento detalhadas no [guia de migração](docs/migration/001-validacoes-mais-rigidas.md).

#### Corrigido
- **[COMPORTAMENTO]** `JsonExtensions`: a serialização/desserialização usava
  `JsonConvert.SerializeObject`/`DeserializeObject` sem `JsonSerializerSettings` completos,
  herdando `JsonConvert.DefaultSettings` global configurado pela aplicação host — se o host
  registrasse `TypeNameHandling.All` (ou similar) globalmente, `"...".Json<T>()` ficava exposto a
  desserialização de tipos arbitrários via `$type` (RCE) e sem limite de profundidade de
  aninhamento. Adicionado `CreateDefaultSettings()` interno, aplicado a `Json<T>()` e ao overload
  `Json(obj, formatting, contractResolver)`, fixando `TypeNameHandling.None` e `MaxDepth = 64`
  explicitamente — não são mais influenciados por `DefaultSettings` global. O overload que recebe
  `JsonSerializerSettings` explícito permanece intocado (usuário no controle). Afeta apenas
  hosts que registrem `DefaultSettings` globais perigosos e dependam do `JsonExtensions` para
  desserialização.
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
  ao de antes para esse caso específico. Se o seu código converte strings com vírgula decimal
  via `Cast<T>`, faça o parse explícito com a cultura desejada antes de atualizar.

As correções de validação (`IsCpf`, `IsZipCode`, `IsPhone`, `IsEmail`, `[Email]`) apenas passam
a rejeitar entradas que antes eram aceitas indevidamente — sentido seguro para quem valida
entrada de usuário. Quem dependia do comportamento anterior (aceitar entradas malformadas) deve
tratar os dados antes de atualizar; ver o [guia de migração](docs/migration/001-validacoes-mais-rigidas.md).

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

### [2.0.0] - 2026-07-14

Mudanças de comportamento e remoções detalhadas no
[guia de migração 004](docs/migration/004-thunder-2.0.md).

#### Removido
- **`CfgSerialization`** e **`SessionManager.SerializeConfiguration`** (descontinuados na 1.3.0):
  removidos. O cache binário de configuração foi desativado por segurança (CWE-502, ver
  [guia 002](docs/migration/002-binaryformatter-desativado.md)) e não tem substituto — a
  configuração é sempre reconstruída em memória.

#### Corrigido
- **[COMPORTAMENTO]** `SessionManager` thread-safe: a inicialização da configuração/fábrica de
  sessões passa a ser feita via `Lazy`, eliminando condições de corrida na primeira sessão (antes,
  uma inicialização parcial podia deixar o `SessionManager` num estado inconsistente). Uma falha ao
  construir a configuração/fábrica (por exemplo, banco indisponível durante a subida da aplicação)
  é propagada ao chamador, mas **não é permanente**: o acesso seguinte tenta materializar de novo.
  Uma vez materializadas com sucesso, as instâncias permanecem as mesmas por todo o processo.
- **[COMPORTAMENTO]** `OrderBy` (extensão de `IQueryOver`): aplica todas as chaves de ordenação
  informadas; antes o método não gerava ordenação alguma na consulta resultante.
- **[COMPORTAMENTO]** Listener de timestamps (`Created`/`Updated`): no `insert`, o servidor sempre
  grava `Created` e `Updated`, ignorando qualquer valor definido manualmente na entidade; no
  `update`, o listener grava `Updated` e simplesmente não altera `Created` — não há proteção
  contra o consumidor alterar `Created` em memória e persistir o novo valor. O horário gravado
  continua sendo o horário local.

#### Alterado
- **[COMPORTAMENTO]** O total de itens da paginação passa de `int` para `long`, para não truncar
  contagens grandes. Consumidores que declaram explicitamente o tipo do total precisam recompilar.

#### Adicionado
- API assíncrona no `Repository`: métodos `...Async` com suporte a `CancellationToken`. Os
  métodos síncronos e a transação-por-método permanecem inalterados. As sobrecargas de conveniência
  não têm variante async. **Ressalva:** os novos membros foram adicionados também a `IRepository` —
  não-breaking para chamadores e para quem herda de `Repository`, mas **breaking para quem
  implementa `IRepository` diretamente**, que precisa implementar os novos membros.

#### Descontinuado
- **`ActiveRecord<T, TKey>`**: marcado `[Obsolete]`. Passa a delegar ao `Repository` e permanece
  funcional; remoção planejada para a 3.0. A delegação traz duas paridades de comportamento com o
  `Repository`, além de uma mudança na declaração da classe:
  - **[COMPORTAMENTO]** Os métodos estáticos de persistência passam a aparar espaços (`Trim()`)
    de todas as strings graváveis da entidade antes de persistir — comportamento que o
    `Repository` sempre teve e o `ActiveRecord` não tinha.
  - **[COMPORTAMENTO]** A sessão passa a ser resolvida via sessão corrente, aberta e vinculada
    automaticamente ao contexto quando necessário — antes era exigida uma sessão já vinculada.
  - A classe base mudou de `ICreatedAndUpdatedProperty where T : class` para `Persist<T, TKey>`
    com a constraint `where T : Persist<T, TKey>` — breaking para declarações fora do padrão CRTP
    (isto é, quando `T` não é a própria classe que herda de `ActiveRecord`).

### [1.3.0] - 2026-07-14

#### Corrigido
- **[COMPORTAMENTO]** Cache binário de configuração desativado: `SessionManager.Configuration`
  podia deserializar um arquivo de cache em disco via `BinaryFormatter` (através de
  `CfgSerialization`) quando a AppSetting `Thunder.Data.SessionManager.SerializeConfiguration`
  estava habilitada — um arquivo de cache adulterado, ou substituído por quem tivesse acesso de
  escrita ao diretório da aplicação, permitia execução arbitrária de código na desserialização
  (CWE-502). `Configuration` agora sempre reconstrói a configuração via `new Configuration()`
  seguido de `Configure()`, sem consultar nem gravar qualquer cache serializado, independentemente
  do valor da AppSetting.

#### Descontinuado
- **`SessionManager.SerializeConfiguration`**: marcada `[Obsolete]`; não tem mais efeito sobre
  `Configuration`.
- **`CfgSerialization`**: marcada `[Obsolete]`; `Create` e `Load()`/`Load(string)` agora lançam
  `NotSupportedException` em vez de serializar/desserializar o arquivo de cache.
- Impacto detalhado e passo a passo de remoção da AppSetting no
  [guia de migração 002](docs/migration/002-binaryformatter-desativado.md).

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

### [2.0.0] - 2026-07-14

#### Alterado
- Alinhamento à linha 2.0: passa a depender de `Thunder 2.0.0`. Não há mudança de comportamento
  própria neste pacote.

### [1.9.0] - 2026-07-14

#### Corrigido
- **[COMPORTAMENTO]** `DecimalModelBinder`: lançava `NullReferenceException` quando o
  `ValueProviderResult` era `null` (campo ausente no request) e não tratava `OverflowException`
  para entradas numéricas fora da faixa de `decimal`; agora retorna `null` com segurança nos dois
  casos, adicionando um erro de model state amigável para valor inválido/overflow em vez de deixar
  a exceção propagar. O parsing continua na cultura corrente da requisição (formato pt-BR
  preservado).
- **[COMPORTAMENTO]** `NotifyBuilder.Builder`/`MessageExtensions`: as mensagens de notificação
  (usadas por `Notify` e pelos helpers `Success`/`Attention`/`Information`/`Error` do tema
  SimplaAdmin) eram inseridas diretamente no HTML de saída sem encode — uma mensagem contendo
  marcação (por exemplo, repetindo entrada do usuário) era renderizada como HTML/script. Passam
  agora por `HttpUtility.HtmlEncode` antes de compor a saída.
- **[COMPORTAMENTO]** `ContentBoxExtensions`: o texto de `ContentBoxHeaderButton.Text` e
  `ContentBoxHeaderTab.Text` era atribuído a `TagBuilder.InnerHtml` sem encode, permitindo XSS via
  texto de botão/aba do content box; corrigido para `TagBuilder.SetInnerText`, que aplica encode
  automaticamente.
- **[COMPORTAMENTO]** `JsonResult`/`NotifyResult` — JSONP: o valor do parâmetro `callback` da
  querystring era ecoado de volta na resposta sem validação, permitindo injeção de script
  refletido (ex. `callback=alert(document.cookie)//`). O nome do callback agora é validado contra
  a expressão regular `\A[A-Za-z_$][A-Za-z0-9_$.\[\]]*\z` (âncoras absolutas de início e fim), a
  resposta é prefixada com `/**/` e o `ContentType` passa a `application/javascript`. Falha segura:
  um callback inválido faz a resposta permanecer como JSON puro, sem o wrapper JSONP.

#### Adicionado
- Host canônico opt-in via a nova AppSetting `Thunder.Web.Mvc.CanonicalHost` (mitiga host header
  poisoning): quando configurada, os métodos de `UrlHelperExtensions` (`AbsoluteAction`,
  `AbsoluteRouteUrl`, `AbsoluteContent`) compõem a autoridade das URLs absolutas a partir do valor
  configurado, em vez de confiar no cabeçalho `Host` da requisição recebida. Aplica-se inclusive a
  URLs já absolutas geradas pelos overloads que recebem `protocol` sem `hostName` explícito, onde
  o `Request.Url.Host` do próprio ASP.NET MVC vazava para a resposta antes desta correção. Recurso
  opt-in: sem a AppSetting configurada, o comportamento é preservado byte a byte.

### [1.8.0] - 2026-07-14

Mudanças de comportamento detalhadas no [guia de migração](docs/migration/001-validacoes-mais-rigidas.md).

#### Corrigido
- `SelectListExtensions.ToSelectList<T>()`: lançava `IndexOutOfRangeException` para enums sem
  `[Display]`; passou a usar `EnumExtensions.DisplayName()`, com fallback para o nome do membro.
- **[COMPORTAMENTO]** `Controller.Success(data, contentType)`: o parâmetro `contentType` era
  ignorado e o overload sempre sobrescrevia para `"application/json"`; agora o valor informado é
  respeitado.
- **[COMPORTAMENTO]** `ModelStateDictionaryExtensions.ExcludePropertiesWithKeyPart`: o predicado
  estava invertido (`ignoreKeys != null && !ignoreKeys.Contains(...)`), tornando o método um
  no-op sempre que `ignoreKeys` era `null` — o caso mais comum, sem lista de exceções; corrigido
  para `ignoreKeys == null || !ignoreKeys.Contains(...)`.
- **[COMPORTAMENTO]** `CompressAttribute`: o cabeçalho `Vary` era emitido com o valor
  `Content-Encoding` (cabeçalho de resposta, sem efeito para caches/proxies), quando deveria
  referenciar `Accept-Encoding` (cabeçalho de requisição que de fato varia a resposta gzip);
  corrigido. Mudança segura para quem registra o filtro globalmente — apenas corrige um
  cabeçalho incorreto, sem alterar a compressão em si.
- **[COMPORTAMENTO]** `NotifyResult` (renderização HTML, caminho não-Ajax): `Messages` é uma
  `IList<KeyValuePair<string, IList<string>>>`; o caminho HTML imprimia
  `KeyValuePair.ToString()` em vez do texto da mensagem. Corrigido para extrair as mensagens de
  cada `Value` e aplicar `HttpUtility.HtmlEncode` em cada uma antes de montar o
  `<li>`/corpo — mitigação pontual de XSS neste caminho específico; o encode abrangente de toda
  a superfície de mensagens fica para uma versão futura.

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

### [2.0.0] - 2026-07-14

Mudanças de comportamento detalhadas no
[guia de migração 004](docs/migration/004-thunder-2.0.md).

#### Alterado
- **[COMPORTAMENTO]** `Repository<T, TKey>` redesenhado com unit-of-work explícito: `Add`, `Update`
  e `Delete` não persistem sozinhos; as mudanças só são confirmadas ao chamar `Save()` ou
  `SaveAsync()`. Antes, cada mutação persistia individualmente.
- **[COMPORTAMENTO]** Ownership correto do `DbContext`: o repositório não descarta mais o
  `DbContext` recebido — deixou de implementar `IDisposable`. O ciclo de vida do contexto é
  responsabilidade de quem o cria (tipicamente o container de injeção de dependência).
- **[COMPORTAMENTO]** As leituras (`All`/`FindBy`) passam a usar `AsNoTracking`. As entidades
  retornadas não são rastreadas pelo contexto; para atualizá-las, use `Update` explicitamente antes
  de `Save`.

#### Adicionado
- `SaveAsync`, `SingleAsync` e `Delete(TKey)`. **Ressalva:** o redesenho de `IRepository` (novos
  membros e nova semântica de persistência) é **breaking para quem implementa a interface
  diretamente**; para chamadores e para quem herda de `Repository`, valem apenas as mudanças de
  comportamento descritas acima.

### [1.1.1] - 2026-07-14

#### Alterado
- Atualização de dependências (patch, sem mudança de API):
  - EntityFramework 6.5.1 → 6.5.2

### [1.1.0]

- Dependências alinhadas às versões em uso pelos consumidores: EntityFramework 6.5.1.
