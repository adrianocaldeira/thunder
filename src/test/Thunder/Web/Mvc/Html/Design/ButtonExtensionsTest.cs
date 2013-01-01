using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class ButtonExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"button\" type=\"submit\" value=\"Save\" />", htmlHelper.Button("Save").ToHtmlString());
            Assert.AreEqual("<input class=\"button\" id=\"save-button\" type=\"submit\" value=\"Save\" />", htmlHelper.Button("Save", new { id = "save-button" }).ToHtmlString());
            Assert.AreEqual("<input class=\"button\" title=\"Save\" type=\"submit\" value=\"Save\" />", htmlHelper.Button("Save", "Save").ToHtmlString());
            Assert.AreEqual("<input class=\"button\" id=\"save-button\" title=\"Save\" type=\"submit\" value=\"Save\" />", htmlHelper.Button("Save", "Save", new { id = "save-button" }).ToHtmlString());
        }
    }
}
