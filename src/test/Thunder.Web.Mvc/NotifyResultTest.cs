using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace Thunder.Web.Mvc
{
    [TestFixture]
    public class NotifyResultTest
    {
        private static ControllerContext CreateNonAjaxControllerContext(StringBuilder written)
        {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(r => r.Headers).Returns(new NameValueCollection());

            var responseMock = new Mock<HttpResponseBase>();
            responseMock.Setup(r => r.Write(It.IsAny<string>())).Callback<string>(s => written.Append(s));

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);
            httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);

            return new ControllerContext(httpContextMock.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

        [Test]
        public void ExecuteResult_UmaMensagem_ImprimeTextoDaMensagemEncodado()
        {
            var written = new StringBuilder();
            var controllerContext = CreateNonAjaxControllerContext(written);
            var result = new NotifyResult(NotifyType.Success, new List<string> { "<script>alert(1)</script>" });

            result.ExecuteResult(controllerContext);

            var html = written.ToString();

            StringAssert.Contains("&lt;script&gt;", html);
            StringAssert.DoesNotContain("System.Collections.Generic.KeyValuePair", html);
            StringAssert.DoesNotContain("<script>alert(1)</script>", html);
        }

        [Test]
        public void ExecuteResult_VariasMensagens_ImprimeCadaMensagemEmLiEncodado()
        {
            var written = new StringBuilder();
            var controllerContext = CreateNonAjaxControllerContext(written);
            var result = new NotifyResult(NotifyType.Danger, new List<string> { "Primeira <b>mensagem</b>", "Segunda mensagem" });

            result.ExecuteResult(controllerContext);

            var html = written.ToString();

            StringAssert.Contains("<li>Primeira &lt;b&gt;mensagem&lt;/b&gt;</li>", html);
            StringAssert.Contains("<li>Segunda mensagem</li>", html);
            StringAssert.DoesNotContain("System.Collections.Generic.KeyValuePair", html);
        }
    }
}
