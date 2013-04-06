using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Moq;
using NUnit.Framework;
using Thunder.Resources;

namespace Thunder
{
    [TestFixture]
    class UtilityTest
    {
        [Test]
        public void GetPropertyName()
        {
            Expression<Func<TireItem, object>> expression1 = item => item.Id;
            Expression<Func<TireItem, object>> expression2 = item => item.State;

            Assert.AreEqual("Id", Utility.GetPropertyName(expression1));
            Assert.AreEqual("State", Utility.GetPropertyName(expression2));
        }

        [Test]
        public void ToAbsolute()
        {
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost", UriKind.Absolute));
            request.SetupGet(x => x.ApplicationPath).Returns("/app");

            Assert.AreEqual("/app/page", Utility.ToAbsolute("~/page", context.Object));
            Assert.AreEqual("http://localhost/page", Utility.ToAbsolute("/page", request.Object).ToString());
            Assert.AreEqual("http://localhost/page", Utility.ToAbsolute("http://localhost/page", request.Object).ToString());
        }
    }
}
