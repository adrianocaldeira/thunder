using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;
using Thunder.Web.Mvc.Html.Design.SimplaAdmin;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class MessageExtensionsTest
    {
        [Test]
        public void Success_UmaMensagem_CodificaHtmlDaMensagem()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            var html = htmlHelper.Success("<script>alert(1)</script>").ToHtmlString();

            StringAssert.Contains("&lt;script&gt;", html);
            StringAssert.DoesNotContain("<script>alert(1)</script>", html);
        }

        [Test]
        public void Error_VariasMensagens_CodificaCadaMensagemEmLi()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            var html = htmlHelper.Error(new List<string> { "Primeira <b>mensagem</b>", "Segunda mensagem" }).ToHtmlString();

            StringAssert.Contains("<li>Primeira &lt;b&gt;mensagem&lt;/b&gt;</li>", html);
            StringAssert.Contains("<li>Segunda mensagem</li>", html);
            StringAssert.DoesNotContain("<b>mensagem</b>", html);
        }

        [Test]
        public void Information_MensagemTextoPuro_MantemTextoLegivel()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            var html = htmlHelper.Information("Salvo com sucesso").ToHtmlString();

            StringAssert.Contains("Salvo com sucesso", html);
        }
    }
}
