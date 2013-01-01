using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class DateExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"text-input date\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"\" />",
                htmlHelper.Date("user").ToHtmlString());

            Assert.AreEqual("<input class=\"text-input date\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Date("user", "11/23/2012").ToHtmlString());

            Assert.AreEqual("<input class=\"text-input date\" maxlength=\"15\" name=\"user\" type=\"text\" value=\"\" />",
                htmlHelper.Date("user", 15).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input date\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Date("user", "11/23/2012", 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input date date-picker\" id=\"date\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Date("user", "11/23/2012", new { id = "date", @class = "date-picker" }).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input date date-picker\" id=\"date\" maxlength=\"20\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Date("user", "11/23/2012", 20, new { id = "date", @class = "date-picker" }).ToHtmlString());
        }
    }
}
