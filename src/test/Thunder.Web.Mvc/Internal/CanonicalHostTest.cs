using System.Configuration;
using System.Reflection;
using NUnit.Framework;

namespace Thunder.Web.Mvc.Internal
{
    [TestFixture]
    public class CanonicalHostTest
    {
        private const string AppSettingKey = "Thunder.Web.Mvc.CanonicalHost";

        private static readonly FieldInfo ValueField =
            typeof(CanonicalHost).GetField("_value", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly FieldInfo LoadedField =
            typeof(CanonicalHost).GetField("_loaded", BindingFlags.NonPublic | BindingFlags.Static);

        private object _originalValue;
        private object _originalLoaded;
        private string _originalAppSetting;

        [SetUp]
        public void Setup()
        {
            _originalValue = ValueField.GetValue(null);
            _originalLoaded = LoadedField.GetValue(null);
            _originalAppSetting = ConfigurationManager.AppSettings[AppSettingKey];

            ResetCache();
        }

        [TearDown]
        public void TearDown()
        {
            ValueField.SetValue(null, _originalValue);
            LoadedField.SetValue(null, _originalLoaded);

            // Nota: ConfigurationManager.AppSettings.Remove(...) lança ConfigurationErrorsException
            // ("a configuração é somente leitura") neste ambiente de testes. Set(key, null) é o
            // caminho suportado para simular "AppSetting ausente" e também para restaurar o estado original.
            ConfigurationManager.AppSettings.Set(AppSettingKey, _originalAppSetting);
        }

        [Test]
        public void Value_SemAppSetting_RetornaNull()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, null);

            Assert.IsNull(CanonicalHost.Value);
        }

        [Test]
        public void Value_ComAppSettingVazio_RetornaNull()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, string.Empty);

            Assert.IsNull(CanonicalHost.Value);
        }

        [Test]
        public void Value_ComAppSettingConfigurado_RetornaValorConfigurado()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com");

            Assert.AreEqual("app.exemplo.com", CanonicalHost.Value);
        }

        [Test]
        public void Value_LeSomenteUmaVez_UsaCacheEmChamadasSubsequentes()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com");

            Assert.AreEqual("app.exemplo.com", CanonicalHost.Value);

            ConfigurationManager.AppSettings.Set(AppSettingKey, "outro-host.exemplo.com");

            Assert.AreEqual("app.exemplo.com", CanonicalHost.Value, "Valor deveria permanecer em cache após a primeira leitura.");
        }

        private static void ResetCache()
        {
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);
        }
    }
}
