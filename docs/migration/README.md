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

_Nenhum até o momento. As versões 1.8.1 / 1.2.1 / 1.7.1 / 1.1.1 são patches de dependência
sem mudança de comportamento._
