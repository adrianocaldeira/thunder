using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html
{
    [TestFixture]
    public class StyleSheetExtensionsTest
    {
        [Test]
        public void Test()
        {
            const string expected = "<link href=\"teste.css\" media=\"screen\" rel=\"stylesheet\" type=\"text/css\" />";
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());
            var result = htmlHelper.StyleSheet("teste.css");

            Assert.AreEqual(expected, result.ToHtmlString());
        }
    }
}
