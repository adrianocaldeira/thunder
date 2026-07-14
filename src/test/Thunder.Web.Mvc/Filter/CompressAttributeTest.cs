using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace Thunder.Web.Mvc.Filter
{
    [TestFixture]
    public class CompressAttributeTest
    {
        [Test]
        public void OnActionExecuting_ComGZipSuportado_AdicionaHeaderVaryComAcceptEncoding()
        {
            var appendedHeaders = new List<KeyValuePair<string, string>>();

            var requestHeaders = new NameValueCollection { { "Accept-Encoding", "gzip" } };
            var requestMock = new Mock<HttpRequestBase>();
            requestMock.Setup(r => r.Headers).Returns(requestHeaders);

            var responseMock = new Mock<HttpResponseBase>();
            responseMock.SetupProperty(r => r.Filter, (Stream) new MemoryStream());
            responseMock.Setup(r => r.Headers).Returns(new NameValueCollection());
            responseMock.Setup(r => r.AppendHeader(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((name, value) => appendedHeaders.Add(new KeyValuePair<string, string>(name, value)));

            var httpContextMock = new Mock<HttpContextBase>();
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);
            httpContextMock.Setup(c => c.Response).Returns(responseMock.Object);

            var controllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var filterContext = new ActionExecutingContext(controllerContext, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());

            var attribute = new CompressAttribute();
            attribute.OnActionExecuting(filterContext);

            Assert.IsTrue(appendedHeaders.Contains(new KeyValuePair<string, string>("Vary", "Accept-Encoding")));
            Assert.IsFalse(appendedHeaders.Contains(new KeyValuePair<string, string>("Vary", "Content-Encoding")));
        }
    }
}
