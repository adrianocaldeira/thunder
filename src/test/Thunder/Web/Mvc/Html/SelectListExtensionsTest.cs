using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;

namespace Thunder.Web.Mvc.Html
{
    [TestFixture]
    public class SelectListExtensionsTest
    {
        [Test]
        public void Test()
        {
            var list = new List<int> {0, 1, 2, 3};

            var items = list.ToSelectList(x => x.ToString(), x => x.ToString(), "-1",
                            new SelectListItem { Selected = true, Text = "Selecione", Value = "-1" });

            Assert.AreEqual(5, items.Count);
        }
    }
}
