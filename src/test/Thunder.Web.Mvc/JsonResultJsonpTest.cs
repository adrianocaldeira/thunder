using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace Thunder.Web.Mvc
{
    [TestFixture]
    public class JsonResultJsonpTest
    {
        private static ControllerContext CreateControllerContext(StringBuilder written, string callback)
        {
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(r => r["callback"]).Returns(callback);

            var responseMock = new Mock<HttpResponseBase>();
            responseMock.SetupProperty(r => r.ContentType);
            responseMock.Setup(r => r.Write(It.IsAny<string>())).Callback<string>(s => written.Append(s));

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);
            httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);

            return new ControllerContext(httpContextMock.Object, new RouteData(), new Mock<ControllerBase>().Object);
        }

        [Test]
        public void ExecuteResult_CallbackValido_EnvolveComComentarioEDefineContentTypeJavascript()
        {
            var written = new StringBuilder();
            var context = CreateControllerContext(written, "foo");
            var result = new JsonResult { Data = new { ok = true } };

            result.ExecuteResult(context);

            var body = written.ToString();

            StringAssert.StartsWith("/**/foo(", body);
            Assert.AreEqual("application/javascript", context.HttpContext.Response.ContentType);
        }

        [Test]
        public void ExecuteResult_CallbackInvalido_NaoEnvolveEMantemContentTypeJson()
        {
            var written = new StringBuilder();
            var context = CreateControllerContext(written, "alert(1)");
            var result = new JsonResult { Data = new { ok = true } };

            result.ExecuteResult(context);

            var body = written.ToString();

            StringAssert.DoesNotContain("alert(1)(", body);
            Assert.AreEqual("application/json", context.HttpContext.Response.ContentType);
        }

        [Test]
        public void ExecuteResult_SemCallback_RetornaJsonPuro()
        {
            var written = new StringBuilder();
            var context = CreateControllerContext(written, null);
            var result = new JsonResult { Data = new { ok = true } };

            result.ExecuteResult(context);

            var body = written.ToString();

            StringAssert.DoesNotContain("(", body);
            Assert.AreEqual("application/json", context.HttpContext.Response.ContentType);
        }
    }
}
