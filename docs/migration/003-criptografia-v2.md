# 003 — Nova API de criptografia (AesEncryptor / PasswordHasher)

**Pacotes afetados:** Thunder 1.10.0
**Tipo de versão:** minor (API nova adicionada; a API legada permanece disponível, marcada
`[Obsolete]`)

## Por que migrar

A API de criptografia legada (`Cryptography`, `CryptographyHelper.Encrypt`/`Decrypt`) e a de hash
(`Hash`, `HashHelper.Hash`) têm fragilidades estruturais que não podem ser corrigidas sem quebrar
compatibilidade com dados já cifrados/hasheados por elas:

- **IV fixo**: `Cryptography` usa o mesmo vetor de inicialização (IV), embutido no código, para
  toda cifragem, independentemente da chave ou do texto. Isso permite análise de padrão entre
  mensagens cifradas com a mesma chave — duas entradas iguais sempre produzem a mesma saída.
- **Derivação de chave fraca**: a chave é derivada com `Rfc2898DeriveBytes` sem controle explícito
  de iterações, tornando ataques de força bruta/dicionário contra a chave mais viáveis do que com
  uma contagem de iterações adequada ao hardware atual.
- **Sem autenticação (CBC sem MAC)**: o texto cifrado não carrega nenhuma verificação de
  integridade. Um payload cifrado pode ser adulterado (bit-flipping em modo CBC) sem que a
  aplicação detecte a adulteração antes de usar o resultado decifrado.
- **Hash legado sem salt por padrão**: `Hash`/`HashHelper.Hash` (provedor padrão SHA-1) não é
  adequado para senhas — sem salt, é vulnerável a rainbow tables, e SHA-1 é rápido demais para
  resistir a força bruta em hardware moderno.

## A nova API

### `Thunder.Security.AesEncryptor` — cifra reversível autenticada

Para dados que precisam ser recuperados em texto original (não senhas): AES-256 em modo CBC, com
IV e salt aleatórios gerados a cada chamada de `Encrypt`, chave derivada por PBKDF2-SHA256 (600.000
iterações) e autenticação Encrypt-then-MAC via HMAC-SHA256 sobre versão, salt, IV e texto cifrado.
A verificação do MAC (em tempo constante) acontece antes de qualquer tentativa de descriptografia
— isso evita ataques de padding oracle e garante que uma chave errada ou payload adulterado sempre
lance `CryptographicException`, nunca retorne texto corrompido silenciosamente.

```csharp
// Antes (API legada)
var cifrado = texto.Encrypt(chave);
var original = cifrado.Decrypt(chave);

// Depois
var cifrado = AesEncryptor.Encrypt(texto, chave);
var original = AesEncryptor.Decrypt(cifrado, chave);
```

### `Thunder.Security.PasswordHasher` — hash de senha (caminho recomendado para senhas)

Para senhas, o caminho recomendado não é cifrar de forma reversível — é gerar um hash que nunca
precisa (nem deve) ser revertido. `PasswordHasher.Hash` usa PBKDF2-SHA256 com salt aleatório de 16
bytes e 600.000 iterações, retornando uma string autodescritiva que já carrega os parâmetros
usados (iterações, salt e subchave, prontos para persistir em um único valor).
`PasswordHasher.Verify` rederiva a subchave com o mesmo salt/iterações do hash informado e compara
em tempo constante; nunca lança exceção para senha errada ou hash malformado, apenas retorna
`false`.

```csharp
// Antes (API legada — hash sem salt, inadequado para senha)
var hash = senha.Hash();
var confere = hash == senhaDigitada.Hash();

// Depois
var hash = PasswordHasher.Hash(senha);
var confere = PasswordHasher.Verify(senhaDigitada, hash);
```

Se o requisito for de fato reversível (por exemplo, armazenar uma credencial de terceiro sistema
que a aplicação precisa reenviar em texto claro), use `AesEncryptor` — não `PasswordHasher`, que é
hash e não é reversível por design.

## AVISO CRÍTICO — incompatibilidade de dados

**A nova API não é compatível com dados cifrados/hasheados pela API legada.** Os formatos de
payload são diferentes (a nova API embute versão, salt, IV e MAC no próprio payload; o hash de
senha embute iterações e salt no próprio valor) e os parâmetros de derivação de chave também
mudam. Não existe conversão direta de um payload/hash antigo para o novo formato sem
descriptografar/validar primeiro com a API antiga.

A migração de dados existentes deve ser tratada como um processo controlado e explícito, não
automático:

1. **Não remova a API legada de uma vez.** `Cryptography`, `CryptographyHelper`, `Hash` e
   `HashHelper` continuam disponíveis (marcadas `[Obsolete]`, mas funcionais) justamente para
   permitir a leitura de dados legados durante a transição. Remoção planejada para a versão 2.0 —
   trate o período até lá como a janela de migração.
2. **Para dados cifrados (`Cryptography`/`CryptographyHelper`)**: em um processo controlado
   (rotina de migração/job dedicado, não sob demanda em runtime de produção), decifre cada valor
   existente com a API antiga (`CryptographyHelper.Decrypt`) e recifre com
   `AesEncryptor.Encrypt`, gravando o novo payload no lugar do antigo.
3. **Para senhas (`Hash`/`HashHelper`)**: hash não é reversível — não é possível recuperar a senha
   original a partir do hash antigo. A estratégia usual é migração progressiva: mantenha o hash
   antigo até o próximo login bem-sucedido do usuário; nesse momento, valide a senha informada
   contra o hash antigo (reproduzindo o cálculo com `HashHelper.Hash` e comparando o resultado) e,
   se conferir, gere e persista um novo hash com `PasswordHasher.Hash`, substituindo o antigo.
4. **Audite todos os pontos de leitura/gravação** que usam as APIs legadas antes de considerar a
   migração concluída — a coexistência das duas APIs durante a transição é esperada, mas cada dado
   deve eventualmente passar para o novo formato antes da remoção na 2.0.

## Referências

Detalhes da adição estão na entrada do [CHANGELOG](../../CHANGELOG.md), seção Thunder 1.10.0.
