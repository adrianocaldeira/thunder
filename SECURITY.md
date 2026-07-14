# Política de segurança

## Versões suportadas

Recebem correções de segurança as versões mais recentes de cada pacote na linha 1.x:

| Pacote | Versão suportada |
|---|---|
| Thunder | 1.10.x |
| Thunder.NHibernate | 1.3.x |
| Thunder.Web.Mvc | 1.9.x |
| Thunder.EntityFramework | 1.1.x |

Versões anteriores podem não receber correções. Recomenda-se manter os pacotes atualizados.

## Como reportar uma vulnerabilidade

Se você encontrou uma possível vulnerabilidade, **não abra uma issue pública**. Reporte de forma privada:

- Por [Security Advisory do GitHub](https://github.com/adrianocaldeira/thunder/security/advisories/new) (preferencial), ou
- Por e-mail ao mantenedor do repositório.

Inclua, quando possível: descrição do problema, pacote e versão afetados, passos para reproduzir, e o impacto potencial.

## O que esperar

- Confirmação de recebimento em até alguns dias úteis.
- Avaliação da severidade e, quando procedente, correção em uma versão de patch/minor, com crédito ao relator se desejado.
- Divulgação coordenada: os detalhes só se tornam públicos após a correção estar disponível.

## Escopo

Este projeto é uma biblioteca utilitária para .NET Framework 4.8. Relatórios mais úteis envolvem:

- Validações e formatações (documentos, entrada de usuário).
- Criptografia e hashing (`Thunder.Security`).
- Componentes web (model binders, filtros, serialização JSON/JSONP, helpers de URL e de mensagens) em `Thunder.Web.Mvc`.
- Integração com NHibernate/Entity Framework.

Boas práticas de uso, como fixar o host canônico em produção e migrar da API de criptografia legada (marcada `[Obsolete]`) para `Thunder.Security.AesEncryptor` / `PasswordHasher`, estão descritas no [CHANGELOG](CHANGELOG.md) e nos guias em [`docs/migration/`](docs/migration/).
