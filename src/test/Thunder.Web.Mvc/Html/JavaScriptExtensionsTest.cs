using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html
{
    [TestFixture]
    public class JavaScriptExtensionsTest
    {
        [Test]
        public void Test()
        {
            const string expected = "<script src=\"teste.js\" type=\"text/javascript\"></script>";
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());
            var result = htmlHelper.JavaScript("teste.js");

            Assert.AreEqual(expected, result.ToHtmlString());
        }
    }
}
