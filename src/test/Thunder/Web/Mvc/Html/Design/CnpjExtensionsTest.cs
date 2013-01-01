using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class CnpjExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"text-input cnpj\" maxlength=\"18\" name=\"user\" type=\"text\" value=\"\" />",
                htmlHelper.Cnpj("user").ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cnpj\" maxlength=\"18\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cnpj("user", "11/23/2012").ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cnpj\" maxlength=\"15\" name=\"user\" type=\"text\" value=\"\" />",
                htmlHelper.Cnpj("user", 15).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cnpj\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cnpj("user", "11/23/2012", 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cnpj cnpj-mask\" id=\"cnpj\" maxlength=\"18\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cnpj("user", "11/23/2012", new { id = "cnpj", @class = "cnpj-mask" }).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cnpj cnpj-mask\" id=\"cnpj\" maxlength=\"18\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cnpj("user", "11/23/2012", 18, new { id = "cnpj", @class = "cnpj-mask" }).ToHtmlString());
        }
    }
}
