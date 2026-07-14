# 002 — Cache binário de configuração do NHibernate desativado

**Pacotes afetados:** Thunder.NHibernate 1.3.0
**Tipo de versão:** minor (comportamento mais seguro; a AppSetting envolvida vira no-op, sem
quebrar compilação)

## O que mudou

`SessionManager.Configuration` podia cachear em disco a configuração do NHibernate já processada,
usando `BinaryFormatter` (via `CfgSerialization`) para serializar e desserializar esse cache. O
recurso era habilitado pela AppSetting `Thunder.Data.SessionManager.SerializeConfiguration`.

Esse cache binário foi desativado:

- `SessionManager.Configuration` sempre reconstrói a configuração via `new Configuration()` seguido
  de `Configure()`, sem consultar nem gravar o arquivo de cache — independentemente do valor da
  AppSetting.
- `CfgSerialization.Create` e `CfgSerialization.Load()`/`Load(string)` agora lançam
  `NotSupportedException` em vez de serializar/desserializar.
- `SessionManager.SerializeConfiguration` e `CfgSerialization` foram marcados `[Obsolete]`.

## Por que mudou

Desserialização binária (`BinaryFormatter`) de um arquivo lido do disco é uma vulnerabilidade
conhecida de execução arbitrária de código (CWE-502): se o arquivo de cache for adulterado, ou
substituído por alguém com qualquer acesso de escrita ao diretório da aplicação (deploy
comprometido, permissão de pasta mal configurada, etc.), a desserialização pode executar código
arbitrário no processo assim que `SessionManager.Configuration` for acessado. O próprio
`BinaryFormatter` é desaconselhado pela plataforma para qualquer cenário que envolva dados não
estritamente confiáveis.

O cache em si trazia um ganho de desempenho limitado: `Configuration.Configure()` já processa
mapeamentos de forma direta, e a configuração resultante continua sendo mantida em memória em um
campo estático enquanto o processo vive — o custo de reconstruí-la ocorre uma única vez por
processo, não a cada acesso.

## Impacto esperado

- **A AppSetting `Thunder.Data.SessionManager.SerializeConfiguration` não tem mais efeito.** Se
  estiver presente no `web.config`/`app.config` com valor `true`, a aplicação continua funcionando
  normalmente — apenas deixa de influenciar o comportamento, sem erro de inicialização.
- **Startup levemente mais lento** em cenários que antes dependiam do cache já existir em disco
  entre reinícios do processo: como a configuração é sempre reconstruída via `Configure()`
  (leitura de mapeamentos, `hibernate.cfg.xml`, etc.), aplicações com muitos mapeamentos podem
  notar um pequeno acréscimo no tempo de inicialização em comparação a quando havia um cache válido
  para reaproveitar. O custo ocorre uma única vez por processo.
- **Qualquer código que referencie `CfgSerialization` diretamente** (fora do uso interno do
  `SessionManager`) passa a receber `NotSupportedException` ao chamar `Create`/`Load`/`Load(string)`.
  Não há mais caminho, nesta API, para reativar o cache binário.

## Passo a passo de migração

1. **Remova a AppSetting `Thunder.Data.SessionManager.SerializeConfiguration`** do
   `web.config`/`app.config`, caso esteja presente — ela não tem mais efeito, e removê-la evita
   confusão futura sobre o que ela controla.
2. **Pare de referenciar `CfgSerialization` diretamente.** Se algum código da aplicação chama
   `CfgSerialization.Create`/`Load`/`Load(string)` fora do `SessionManager` (por exemplo, para
   gerar o cache manualmente em um passo de build/deploy), remova essas chamadas — elas agora
   lançam `NotSupportedException`.
3. **Arquivos de cache binário residuais** no diretório da aplicação (o nome era o informado ao
   construtor de `CfgSerialization`) podem ser removidos com segurança; deixaram de ser lidos.
4. **Nenhuma ação é necessária no mapeamento NHibernate em si** — `Configure()` continua lendo a
   configuração da mesma forma que sempre leu quando o cache estava desabilitado ou expirado.

## Referências

Detalhes da correção estão na entrada marcada com **[COMPORTAMENTO]** no
[CHANGELOG](../../CHANGELOG.md), seção Thunder.NHibernate 1.3.0.
