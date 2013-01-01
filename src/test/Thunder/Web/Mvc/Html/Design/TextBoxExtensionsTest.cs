using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class TextBoxExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"text-input xsmall-input\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"\" />",
                            htmlHelper.TextBox("user", 100, WidthStyle.Xsmall).ToHtmlString());
            
            Assert.AreEqual("<input class=\"text-input small-input\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"\" />",
                            htmlHelper.TextBox("user", 100, WidthStyle.Small).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input medium-input\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"\" />",
                            htmlHelper.TextBox("user", 100, WidthStyle.Medium).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input large-input\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"\" />",
                            htmlHelper.TextBox("user", 100, WidthStyle.Large).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input large-input\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"Adriano\" />",
                            htmlHelper.TextBox("user", "Adriano", 100, WidthStyle.Large).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input large-input\" id=\"user-name\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"Adriano\" />",
                            htmlHelper.TextBox("user", "Adriano", 100, WidthStyle.Large, new {id="user-name"}).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input large-input teste\" id=\"user-name\" maxlength=\"100\" name=\"user\" type=\"text\" value=\"Adriano\" />",
                            htmlHelper.TextBox("user", "Adriano", 100, WidthStyle.Large, new { id = "user-name", @class = "teste" }).ToHtmlString());
        }
    }
}
