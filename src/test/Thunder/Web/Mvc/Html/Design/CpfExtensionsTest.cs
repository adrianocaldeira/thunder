using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;

namespace Thunder.Web.Mvc.Html.Design
{
    [TestFixture]
    public class CpfExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<input class=\"text-input cpf\" maxlength=\"14\" name=\"user\" type=\"text\" value=\"\" />",
                htmlHelper.Cpf("user").ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cpf\" maxlength=\"14\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cpf("user", "11/23/2012").ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cpf\" maxlength=\"15\" name=\"user\" type=\"text\" value=\"\" />",
                htmlHelper.Cpf("user", 15).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cpf\" maxlength=\"10\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cpf("user", "11/23/2012", 10).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cpf cpf-mask\" id=\"cpf\" maxlength=\"14\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cpf("user", "11/23/2012", new { id = "cpf", @class = "cpf-mask" }).ToHtmlString());

            Assert.AreEqual("<input class=\"text-input cpf cpf-mask\" id=\"cpf\" maxlength=\"14\" name=\"user\" type=\"text\" value=\"11/23/2012\" />",
                htmlHelper.Cpf("user", "11/23/2012", 14, new { id = "cpf", @class = "cpf-mask" }).ToHtmlString());
        }
    }
}
