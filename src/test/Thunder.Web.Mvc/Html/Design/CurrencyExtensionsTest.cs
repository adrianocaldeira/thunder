using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class CurrencyExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"text-input currency\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"\" />",
                            htmlHelper.Currency("user", 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input currency\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"1\" />",
                            htmlHelper.Currency("user", 1, 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input currency test\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"1\" />",
                             htmlHelper.Currency("user", 1, 10, new { @class = "test" }).ToHtmlString());
        }
    }
}
