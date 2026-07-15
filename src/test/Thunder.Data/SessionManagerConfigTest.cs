using System;
using System.IO;
using NHibernate.Cfg;
using NUnit.Framework;
using Thunder.NHibernate;

namespace Thunder.Data
{
    [TestFixture]
    public class SessionManagerConfigTest
    {
        [Test]
        public void Configuration_retorna_instancia_valida()
        {
            var configuration = SessionManager.Configuration;

            Assert.IsNotNull(configuration);
            Assert.IsInstanceOf<Configuration>(configuration);
        }

        [Test]
        public void Configuration_e_singleton()
        {
            Assert.AreSame(SessionManager.Configuration, SessionManager.Configuration);
        }

        [Test]
        public void SessionFactory_e_singleton()
        {
            Assert.AreSame(SessionManager.SessionFactory, SessionManager.SessionFactory);
        }

        [Test]
        public void Configuration_nao_cria_cache_binario_legado()
        {
            // Regressão do CWE-502: o cache binário de configuração foi removido e o acesso à
            // Configuration não deve criar (nem depender de) arquivo de cache em disco.
            var legacyCacheFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfg.thunder");

            Assert.IsNotNull(SessionManager.Configuration);
            Assert.False(File.Exists(legacyCacheFile),
                "SessionManager.Configuration não deve criar cache binário de configuração em disco.");
        }
    }
}
