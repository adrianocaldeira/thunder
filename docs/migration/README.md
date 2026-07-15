# Guias de migração

Esta pasta reúne os guias de migração dos pacotes Thunder. Um guia é criado sempre que uma
release contém **mudança de comportamento observável** ou **breaking change** (major), para que
projetos consumidores atualizem com segurança.

## Convenções

- Um arquivo por tema/release sensível, em pt-BR, nomeado `NNN-titulo-curto.md`
  (ex.: `001-cryptography-v2.md`).
- Cada guia contém: o que mudou, por que mudou, impacto esperado, passo a passo de migração
  e, quando aplicável, exemplo de código antes/depois.
- Mudanças que não alteram comportamento (patch de dependência, correção interna) não geram
  guia — ficam registradas apenas no [CHANGELOG](../../CHANGELOG.md).

## Guias disponíveis

- [001 — Validações mais rígidas e correções de comportamento](001-validacoes-mais-rigidas.md)
  (Thunder 1.9.0 / Thunder.Web.Mvc 1.8.0)
- [002 — Cache binário de configuração do NHibernate desativado](002-binaryformatter-desativado.md)
  (Thunder.NHibernate 1.3.0)
- [003 — Nova API de criptografia (AesEncryptor / PasswordHasher)](003-criptografia-v2.md)
  (Thunder 1.10.0)
- [004 — Atualização para a linha 2.0](004-thunder-2.0.md)
  (Thunder 2.0.0 / Thunder.NHibernate 2.0.0 / Thunder.Web.Mvc 2.0.0 / Thunder.EntityFramework 2.0.0)
