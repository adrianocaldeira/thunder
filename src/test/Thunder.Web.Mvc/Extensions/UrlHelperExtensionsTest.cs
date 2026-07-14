using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using Moq;
using NUnit.Framework;
using Thunder.Web.Mvc.Internal;

namespace Thunder.Web.Mvc.Extensions
{
    [TestFixture]
    public class UrlHelperExtensionsTest
    {
        #region ComposeAbsoluteUrl (lógica pura)

        [Test]
        public void ComposeAbsoluteUrl_CanonicalHostNulo_UsaAutoridadeDoRequest()
        {
            var result = UrlHelperExtensions.ComposeAbsoluteUrl(null, "https", "https://evil.com", "/pedidos/1");

            Assert.AreEqual("https://evil.com/pedidos/1", result);
        }

        [Test]
        public void ComposeAbsoluteUrl_CanonicalHostSemEsquema_PrefixaEsquemaDoRequest()
        {
            var result = UrlHelperExtensions.ComposeAbsoluteUrl("app.exemplo.com", "https", "https://evil.com", "/pedidos/1");

            Assert.AreEqual("https://app.exemplo.com/pedidos/1", result);
        }

        [Test]
        public void ComposeAbsoluteUrl_CanonicalHostComEsquema_RespeitaEsquemaInformado()
        {
            var result = UrlHelperExtensions.ComposeAbsoluteUrl("https://app.exemplo.com", "http", "http://evil.com", "/pedidos/1");

            Assert.AreEqual("https://app.exemplo.com/pedidos/1", result);
        }

        [Test]
        public void ComposeAbsoluteUrl_CanonicalHostSemEsquema_PreservaEsquemaHttpDoRequest()
        {
            var result = UrlHelperExtensions.ComposeAbsoluteUrl("app.exemplo.com", "http", "http://evil.com", "/pedidos/1");

            Assert.AreEqual("http://app.exemplo.com/pedidos/1", result);
        }

        #endregion

        #region GetAbsoluteUrl (integração com CanonicalHost.Value via reflection)

        private const string AppSettingKey = "Thunder.Web.Mvc.CanonicalHost";

        private static readonly FieldInfo ValueField =
            typeof(CanonicalHost).GetField("_value", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly FieldInfo LoadedField =
            typeof(CanonicalHost).GetField("_loaded", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo GetAbsoluteUrlMethod =
            typeof(UrlHelperExtensions).GetMethod("GetAbsoluteUrl", BindingFlags.NonPublic | BindingFlags.Static);

        private object _originalValue;
        private object _originalLoaded;
        private string _originalAppSetting;

        [SetUp]
        public void Setup()
        {
            _originalValue = ValueField.GetValue(null);
            _originalLoaded = LoadedField.GetValue(null);
            _originalAppSetting = ConfigurationManager.AppSettings[AppSettingKey];
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

        private static Uri InvokeGetAbsoluteUrl(HttpRequestBase request, string url)
        {
            return (Uri)GetAbsoluteUrlMethod.Invoke(null, new object[] { request, url });
        }

        private static Mock<HttpRequestBase> CreateRequestMock(string absoluteUri)
        {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(r => r.Url).Returns(new Uri(absoluteUri));
            return requestMock;
        }

        [Test]
        public void GetAbsoluteUrl_SemCanonicalHost_UrlJaAbsoluta_RetornaInalterada()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, null);
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("https://evil.com/");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "https://outro.exemplo.com/pagina");

            Assert.AreEqual("https://outro.exemplo.com/pagina", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_ComCanonicalHostConfigurado_UrlAbsolutaMaliciosaTemHostSubstituidoPeloCanonico()
        {
            // Fecha o bypass do A4: overloads AbsoluteAction/AbsoluteRouteUrl com "protocol" (sem hostName)
            // fazem o próprio ASP.NET MVC gerar a url já absoluta usando Request.Url.Host (não confiável).
            // Mesmo assim, com CanonicalHost configurado, a autoridade final deve ser sempre a canônica.
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com");
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("https://evil.com/");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "https://evil.com/Conta/Reset?token=abc");

            Assert.AreEqual("https://app.exemplo.com/Conta/Reset?token=abc", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_ComCanonicalHostConfigurado_PreservaEsquemaHttpDaUrlAbsoluta()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com");
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("https://evil.com/");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "http://evil.com/x");

            Assert.AreEqual("http://app.exemplo.com/x", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_ComCanonicalHostConfigurado_PreservaEsquemaHttpsDaUrlAbsoluta()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com");
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("http://evil.com/");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "https://evil.com/x");

            Assert.AreEqual("https://app.exemplo.com/x", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_SemCanonicalHost_UsaHostDoRequest_ComportamentoAtualPreservado()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, null);
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("https://evil.com/qualquer");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "/pedidos/1");

            Assert.AreEqual("https://evil.com/pedidos/1", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_ComCanonicalHostConfigurado_UsaHostCanonicoEmVezDoRequest()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com");
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("https://evil.com/qualquer");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "/pedidos/1");

            Assert.AreEqual("https://app.exemplo.com/pedidos/1", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_ComCanonicalHostIncluindoEsquema_RespeitaEsquemaConfigurado()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "https://app.exemplo.com");
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("http://evil.com/qualquer");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "/pedidos/1");

            Assert.AreEqual("https://app.exemplo.com/pedidos/1", result.ToString());
        }

        [Test]
        public void GetAbsoluteUrl_ComCanonicalHostComBarraFinal_NaoGeraBarraDupla()
        {
            ConfigurationManager.AppSettings.Set(AppSettingKey, "app.exemplo.com/");
            ValueField.SetValue(null, null);
            LoadedField.SetValue(null, false);

            var requestMock = CreateRequestMock("https://evil.com/qualquer");

            var result = InvokeGetAbsoluteUrl(requestMock.Object, "/pedidos/1");

            Assert.AreEqual("https://app.exemplo.com/pedidos/1", result.ToString());
        }

        #endregion
    }
}
