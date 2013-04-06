using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class NumericExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"text-input numeric\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"\" />",
                            htmlHelper.Numeric("user", 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input numeric\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"1\" />",
                            htmlHelper.Numeric("user", 1, 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input numeric teste\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"1\" />",
                             htmlHelper.Numeric("user", 1, 10, new { @class = "teste" }).ToHtmlString());
        }
    }
}
