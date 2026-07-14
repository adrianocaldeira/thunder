using System.Collections.Generic;
using NUnit.Framework;

namespace Thunder.Web.Mvc.Html.Notify
{
    [TestFixture]
    public class NotifyBuilderTest
    {
        [Test]
        public void Builder_UmaMensagem_CodificaHtmlDaMensagem()
        {
            var notify = new Thunder.Notify(NotifyType.Success, "<script>alert(1)</script>");
            var builder = new NotifyBuilder();

            var html = builder.Builder(notify, false, null).ToHtmlString();

            StringAssert.Contains("&lt;script&gt;", html);
            StringAssert.DoesNotContain("<script>alert(1)</script>", html);
        }

        [Test]
        public void Builder_VariasMensagens_CodificaCadaMensagemEmLi()
        {
            var notify = new Thunder.Notify(NotifyType.Danger, new List<string> { "Primeira <b>mensagem</b>", "Segunda mensagem" });
            var builder = new NotifyBuilder();

            var html = builder.Builder(notify, false, null).ToHtmlString();

            StringAssert.Contains("<li>Primeira &lt;b&gt;mensagem&lt;/b&gt;</li>", html);
            StringAssert.Contains("<li>Segunda mensagem</li>", html);
            StringAssert.DoesNotContain("<b>mensagem</b>", html);
        }

        [Test]
        public void Builder_MensagemTextoPuro_MantemTextoLegivel()
        {
            var notify = new Thunder.Notify(NotifyType.Success, "Salvo com sucesso");
            var builder = new NotifyBuilder();

            var html = builder.Builder(notify, false, null).ToHtmlString();

            StringAssert.Contains("Salvo com sucesso", html);
        }
    }
}
