using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design.SimplaAdmin
{
    /// <summary>
    /// Testa a codificação de texto livre em botões e abas do content box (A2 — XSS).
    /// </summary>
    /// <remarks>
    /// <c>CreateHeaderButtons</c>/<c>CreateHeaderTabs</c> são privados e chamados internamente por
    /// <c>ContentBoxExtensions.ContentBox(...)</c>, que escreve diretamente em
    /// <c>htmlHelper.ViewContext.Writer</c>. O <c>MockHtmlHelper.Make</c> do pacote Thunder.Mock
    /// (dependência externa, sem código fonte neste repositório) mocka <c>ViewContext</c> via Moq sem
    /// <c>CallBase = true</c>, então <c>Writer</c> (propriedade virtual não configurada via Setup)
    /// retorna <c>null</c> — inviabilizando testar pelo ponto público (<c>ContentBox(...)</c>) sem
    /// alterar o pacote externo. Por isso os métodos privados são exercitados via reflection,
    /// escrevendo em um <see cref="TagBuilder"/> próprio (que não depende do Writer).
    /// </remarks>
    [TestFixture]
    public class ContentBoxExtensionsTest
    {
        private static readonly MethodInfo CreateHeaderButtonsMethod =
            typeof(ContentBoxExtensions).GetMethod("CreateHeaderButtons", BindingFlags.NonPublic | BindingFlags.Static);

        private static readonly MethodInfo CreateHeaderTabsMethod =
            typeof(ContentBoxExtensions).GetMethod("CreateHeaderTabs", BindingFlags.NonPublic | BindingFlags.Static);

        [Test]
        public void CreateHeaderButtons_TextoMalicioso_CodificaTextoDoBotao()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());
            var header = new ContentBoxHeader
            {
                Buttons = new List<ContentBoxHeaderButton>
                {
                    new ContentBoxHeaderButton { Text = "<script>alert(1)</script>", Url = "/painel/editar" }
                }
            };
            var builder = new TagBuilder("div");

            CreateHeaderButtonsMethod.Invoke(null, new object[] { header, builder, htmlHelper });

            var html = builder.InnerHtml;

            StringAssert.Contains("&lt;script&gt;alert(1)&lt;/script&gt;", html);
            StringAssert.DoesNotContain("<script>alert(1)</script>", html);
        }

        [Test]
        public void CreateHeaderTabs_TextoMalicioso_CodificaTextoDaTab()
        {
            var header = new ContentBoxHeader
            {
                Tabs = new List<ContentBoxHeaderTab>
                {
                    new ContentBoxHeaderTab { Text = "<script>alert(1)</script>", Title = "Aba" }
                }
            };
            var builder = new TagBuilder("div");

            CreateHeaderTabsMethod.Invoke(null, new object[] { header, builder });

            var html = builder.InnerHtml;

            StringAssert.Contains("&lt;script&gt;alert(1)&lt;/script&gt;", html);
            StringAssert.DoesNotContain("<script>alert(1)</script>", html);
        }
    }
}
