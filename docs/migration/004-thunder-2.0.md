# 004 — Atualização para a linha 2.0

**Pacotes afetados:** Thunder 2.0.0 · Thunder.NHibernate 2.0.0 · Thunder.Web.Mvc 2.0.0 ·
Thunder.EntityFramework 2.0.0
**Tipo de versão:** major (remoções e mudanças de comportamento). Os quatro pacotes sobem juntos
para 2.0.0; `Thunder.NHibernate`, `Thunder.Web.Mvc` e `Thunder.EntityFramework` passam a depender
de `Thunder 2.0.0`.

Este guia reúne o que muda ao subir de qualquer versão 1.x para a 2.0. As mudanças específicas de
criptografia/hash têm um guia dedicado — ver [003](003-criptografia-v2.md).

## Remoções e substitutos

| Removido | Pacote | Substituto |
| --- | --- | --- |
| `Hash`, `HashHelper` | Thunder | `PasswordHasher.Hash`/`Verify` (hash de senha) |
| `HashProvider` (enum) | Thunder | — (só existia para os tipos acima) |
| `CfgSerialization` | Thunder.NHibernate | — (cache binário desativado por segurança) |
| `SessionManager.SerializeConfiguration` | Thunder.NHibernate | — (sem efeito desde a 1.3.0) |

- **`Hash` / `HashHelper` / `HashProvider`** foram descontinuados na 1.10.0 e agora removidos. O
  provedor padrão era SHA-1 sem salt, inadequado para senhas. Para senhas, use
  `PasswordHasher.Hash`/`Verify`; para cifragem reversível, `AesEncryptor.Encrypt`/`Decrypt`. Não
  há substituto que reproduza o formato de hash antigo — a estratégia de migração de dados já
  hasheados está descrita no [guia 003](003-criptografia-v2.md).
- **`CfgSerialization` e `SessionManager.SerializeConfiguration`** foram descontinuados na 1.3.0 e
  agora removidos. O cache binário de configuração do NHibernate foi desativado por segurança
  (desserialização via `BinaryFormatter`, CWE-502 — ver
  [guia 002](002-binaryformatter-desativado.md)). Não há substituto: a configuração é sempre
  reconstruída em memória. Se ainda houver a AppSetting `Thunder.Data.SessionManager.SerializeConfiguration`
  no `web.config`/`app.config`, remova-a — ela não tem mais efeito.

## Mudanças de comportamento

### Thunder — identidade de entidade (`Persist<T, TKey>`)

- **`IsNew()` com qualquer tipo de chave.** Antes, uma entidade era considerada "nova" quando
  `Id <= 0` — regra válida apenas para chaves numéricas. Agora `IsNew()` funciona para `Guid`,
  `string` e tipos numéricos, considerando nova apenas a entidade cujo `Id` é igual ao valor padrão
  do tipo (`default(TKey)`). **Consequência:** um `Id` numérico negativo deixa de ser tratado como
  "novo". Se o seu modelo usa `Id` negativo com algum significado (marcador, seed manual), revise
  os pontos que dependem de `IsNew()`.
- **`Equals` compara `Id` + tipo.** Antes a comparação se baseava no hash code, o que podia
  produzir falsos positivos/negativos. Agora duas entidades são iguais quando têm o mesmo `Id` e o
  mesmo tipo. Revise usos em `HashSet`, `Dictionary` e comparações diretas (`==`, `.Equals`) que
  dependiam do comportamento antigo.
- **`GetHashCode` estável e sem exceção.** Deixa de lançar exceção quando o `Id` é nulo. Uma
  entidade transiente colocada numa coleção baseada em hash **antes** de persistir mantém o hash de
  identidade de referência (não muda o hash ao receber o `Id`), o que preserva a localização do
  item na coleção em cenários detached/multi-sessão.

### Thunder.NHibernate

- **`SessionManager` thread-safe e fail-fast.** A configuração/fábrica de sessões passa a ser
  inicializada via `Lazy`, eliminando condições de corrida na primeira sessão. Uma falha ao
  construir a configuração/fábrica agora é **fail-fast**: a exceção é cacheada e relançada nos
  acessos seguintes, sem retry automático. Se a sua aplicação dependia de que uma configuração
  inválida "se recuperasse" numa chamada posterior, isso não ocorre mais — corrija a causa raiz da
  falha de configuração.
- **Listener de timestamps (`Created`/`Updated`).** No `insert`, o servidor sempre grava `Created`
  e `Updated`, **ignorando** qualquer valor definido manualmente na entidade. No `update`, grava
  apenas `Updated` — `Created` é imutável após a criação. O horário gravado continua sendo o
  horário local. Revise rotinas de importação/carga que definiam `Created` manualmente esperando
  que o valor fosse preservado.
- **`OrderBy` (extensão de `IQueryOver`).** Passa a aplicar todas as chaves de ordenação; antes o
  método não gerava ordenação alguma. Consultas que usavam `OrderBy` e "funcionavam" sem ordenação
  agora retornam resultados ordenados — verifique se alguma lógica dependia da ordem não
  determinística anterior.
- **Total de paginação em `long`.** O total de itens passa de `int` para `long`, para não truncar
  contagens grandes. É uma mudança de assinatura: código que declara explicitamente o tipo do total
  (variável tipada, atribuição a `int`) precisa ser ajustado e recompilado.

### Thunder.EntityFramework — `Repository<T, TKey>`

- **Unit-of-work explícito.** `Add`, `Update` e `Delete` não persistem sozinhos; as mudanças só são
  confirmadas ao chamar `Save()`/`SaveAsync()`. Antes, cada mutação persistia individualmente.
  Ajuste o código para agrupar as mutações e chamar `Save` ao final da operação.
- **Ownership do `DbContext`.** O repositório não descarta mais o `DbContext` recebido — deixou de
  implementar `IDisposable`. O ciclo de vida do contexto passa a ser responsabilidade de quem o cria
  (tipicamente o container de injeção de dependência). Remova `using`/`Dispose` que assumiam que o
  repositório fecharia o contexto.
- **Leituras sem rastreamento.** `All` e `FindBy` passam a usar `AsNoTracking`. As entidades
  retornadas não são rastreadas pelo contexto; para atualizá-las, chame `Update` explicitamente
  antes de `Save`.

### Thunder.Web.Mvc

Sem mudança de comportamento própria. A versão 2.0.0 apenas alinha o pacote à linha 2.0 e passa a
depender de `Thunder 2.0.0`.

## Novidades não-breaking

- **API assíncrona no `Repository` do NHibernate.** Novos métodos `...Async` com suporte a
  `CancellationToken`, aditivos: os métodos síncronos e a transação-por-método continuam
  disponíveis. As sobrecargas de conveniência não têm variante async.
- **API assíncrona no `Repository` do EntityFramework.** Adicionados `SaveAsync`, `SingleAsync`,
  `Delete(TKey)` e as variantes assíncronas correspondentes.
- **`ActiveRecord<T, TKey>` marcado `[Obsolete]`** (Thunder.NHibernate). Continua funcional —
  passa a delegar ao `Repository` — e emite aviso de compilação. Remoção planejada para a 3.0;
  migre para `Repository` no seu próprio ritmo.

## Checklist de atualização

1. **Suba as referências dos quatro pacotes para `2.0.0`** (`packages.config`,
   `PackageReference` ou `.csproj`), mantendo-os na mesma linha de versão.
2. **Substitua os membros removidos antes de compilar:**
   - `Hash`/`HashHelper`/`HashProvider` → `PasswordHasher` (senhas) ou `AesEncryptor` (cifragem).
   - Remova referências a `CfgSerialization`/`SessionManager.SerializeConfiguration` e a AppSetting
     correspondente do `*.config`.
3. **Recompile** e trate os avisos de `[Obsolete]` (`ActiveRecord`) e quaisquer erros de assinatura
   (por exemplo, total de paginação agora `long`).
4. **Valide em runtime** os pontos sensíveis:
   - **Entidades:** comparações de identidade (`Equals`/`GetHashCode`) e uso de `IsNew()`,
     especialmente com chaves não numéricas ou `Id` negativo, e em coleções `HashSet`/`Dictionary`.
   - **Timestamps:** rotinas de criação/importação que dependiam de `Created`/`Updated` definidos
     manualmente.
   - **Paginação:** consultas que usam `OrderBy` (agora efetivamente ordenadas) e o total agora em
     `long`.
   - **Repositório EF:** confirme que cada operação chama `Save`/`SaveAsync`; que o `DbContext` é
     descartado por quem o cria; e que as entidades lidas por `All`/`FindBy` (sem rastreamento)
     passam por `Update` antes de serem persistidas.

## Referências

Entradas correspondentes no [CHANGELOG](../../CHANGELOG.md), seções 2.0.0 de cada pacote.
Guias relacionados: [002 — cache binário do NHibernate](002-binaryformatter-desativado.md) e
[003 — nova API de criptografia](003-criptografia-v2.md).
