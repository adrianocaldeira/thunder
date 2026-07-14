using System;
using System.IO;
using System.Reflection;
using NHibernate.Cfg;
using NUnit.Framework;
using Thunder.NHibernate;

namespace Thunder.Data
{
    [TestFixture]
    public class SessionManagerConfigTest
    {
        private static readonly FieldInfo ConfigurationField =
            typeof(SessionManager).GetField("_configuration", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly FieldInfo SerializeConfigurationField =
            typeof(SessionManager).GetField("_serializeConfiguration", BindingFlags.NonPublic | BindingFlags.Static);

        private object _originalConfiguration;
        private object _originalSerializeConfiguration;

        [SetUp]
        public void Setup()
        {
            _originalConfiguration = ConfigurationField.GetValue(null);
            _originalSerializeConfiguration = SerializeConfigurationField.GetValue(null);
        }

        [TearDown]
        public void TearDown()
        {
            ConfigurationField.SetValue(null, _originalConfiguration);
            SerializeConfigurationField.SetValue(null, _originalSerializeConfiguration);
        }

        [Test]
        public void ConfigurationShouldReturnValidInstanceWithoutBinaryCache()
        {
            ConfigurationField.SetValue(null, null);

            var configuration = SessionManager.Configuration;

            Assert.IsNotNull(configuration);
            Assert.IsInstanceOf<Configuration>(configuration);
        }

        [Test]
        public void ConfigurationShouldIgnoreLegacySerializeConfigurationFlagEvenWhenTrue()
        {
            // Artefato legado de execuções antigas: garante que o teste não dependa do estado do disco.
            var legacyCacheFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cfg.thunder");
            if (File.Exists(legacyCacheFile))
            {
                File.Delete(legacyCacheFile);
            }

            ConfigurationField.SetValue(null, null);
            SerializeConfigurationField.SetValue(null, (bool?)true);

            Configuration configuration = null;

            Assert.DoesNotThrow(() => configuration = SessionManager.Configuration);
            Assert.IsNotNull(configuration);

            Assert.False(File.Exists(legacyCacheFile),
                "SessionManager.Configuration não deve criar/ler cache binário mesmo com a flag legada SerializeConfiguration ativa.");
        }

        [Test]
        public void SerializeConfigurationPropertyShouldBeMarkedObsolete()
        {
            var property = typeof(SessionManager).GetProperty("SerializeConfiguration");
            var attributes = property.GetCustomAttributes(typeof(ObsoleteAttribute), false);

            Assert.IsNotEmpty(attributes);
        }
    }
}
