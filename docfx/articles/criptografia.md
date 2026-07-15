# Criptografia e hash

O pacote **Thunder** expõe duas APIs em `Thunder.Security`: `AesEncryptor`, para criptografia
simétrica reversível, e `PasswordHasher`, para armazenamento de senhas (hash unidirecional).

> As classes legadas `Cryptography` e `CryptographyHelper` estão marcadas como `[Obsolete]`
> (IV fixo e derivação de chave fraca) e serão removidas em uma major futura. Use
> `AesEncryptor` no lugar de `Cryptography`/`CryptographyHelper.Encrypt`/`Decrypt`.

## AesEncryptor: criptografar e decriptografar

`AesEncryptor` usa AES-256-CBC com um HMAC-SHA256 sobre o payload (Encrypt-then-MAC): cada
chamada a `Encrypt` gera um salt e um IV aleatórios, então deriva as chaves de cifra e de MAC
a partir da senha informada.

```csharp
using Thunder.Security;

var cifrado = AesEncryptor.Encrypt("informação confidencial", "senha-super-secreta");
// payload Base64 contendo versão, salt, IV, texto cifrado e MAC

var decifrado = AesEncryptor.Decrypt(cifrado, "senha-super-secreta");
// "informação confidencial"

AesEncryptor.Decrypt(cifrado, "senha-errada");
// lança CryptographicException — a verificação do MAC falha antes de qualquer decriptação,
// então uma senha errada nunca retorna um texto corrompido: ela sempre lança exceção
```

## PasswordHasher: hash de senha

`PasswordHasher` usa PBKDF2-SHA256 (600.000 iterações) com um salt aleatório de 16 bytes. O
resultado de `Hash` é uma string auto-descritiva (`"{iterações}.{salt em Base64}.{subkey em
Base64}"`), pronta para ser persistida e validada depois com `Verify`.

```csharp
using Thunder.Security;

var hash = PasswordHasher.Hash("minhaSenha123");
// "600000.<salt em Base64>.<subkey em Base64>"

PasswordHasher.Verify("minhaSenha123", hash); // true
PasswordHasher.Verify("outraSenha", hash);    // false
```

Um fluxo típico de cadastro/autenticação:

```csharp
// Cadastro
var usuario = new Usuario { Email = "usuario@exemplo.com.br" };
usuario.SenhaHash = PasswordHasher.Hash(senhaInformada);
repositorio.Salvar(usuario);

// Login
var usuario = repositorio.BuscarPorEmail(email);

if (usuario != null && PasswordHasher.Verify(senhaInformada, usuario.SenhaHash))
{
    // autenticado
}
```

`Verify` nunca lança exceção para senha incorreta ou hash malformado — ela simplesmente
retorna `false`, o que a torna segura para uso direto em uma tela de login.
