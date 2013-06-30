using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;
using Thunder.Web.Mvc.Html.Design.SimplaAdmin;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class LabelExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<label for=\"name\">Name:&nbsp;<span class=\"required\">*</span></label>", htmlHelper.Label("name", "Name:", true).ToHtmlString());
            Assert.AreEqual("<label class=\"test\" for=\"name\">Name:&nbsp;<span class=\"required\">*</span></label>", htmlHelper.Label("name", "Name:", true, new { @class = "test" }).ToHtmlString());
        }
    }
}
