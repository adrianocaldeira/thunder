using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Thunder.Mock;
using Thunder.Web.Mvc.Extensions;

namespace Thunder.Web.Mvc
{
    [TestFixture]
    public class UrlHelperExtensionsTest
    {
        [Test]
        public void Absolute()
        {
            var context = MockContext.Make(MockRequest.Make("http://www.integgro.com.br")).Object;
            var helper = new UrlHelper(new RequestContext(context, new RouteData()), new RouteCollection());

            Assert.AreEqual("http://www.integgro.com.br/clients/1/detail", helper.Absolute("/clients/1/detail").ToString());
            Assert.AreEqual("http://www.integgro.com.br/clients/1/detail", helper.Absolute("http://www.integgro.com.br/clients/1/detail").ToString());
        }
    }
}
