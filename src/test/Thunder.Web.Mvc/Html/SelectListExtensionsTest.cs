using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;

namespace Thunder.Web.Mvc.Html
{
    [TestFixture]
    public class SelectListExtensionsTest
    {
        private enum SituacaoSemDisplay
        {
            Ativo,
            Inativo
        }

        [Test]
        public void Test()
        {
            var list = new List<int> {0, 1, 2, 3};

            var items = list.ToSelectList(x => x.ToString(), x => x.ToString(), "-1",
                            new SelectListItem { Selected = true, Text = "Selecione", Value = "-1" });

            Assert.AreEqual(5, items.Count);
        }

        [Test]
        public void ToSelectList_EnumSemDisplay_NaoLancaExcecaoUsaNomeDoMembro()
        {
            var items = SelectListExtensions.ToSelectList<SituacaoSemDisplay>().Cast<SelectListItem>().ToList();

            Assert.AreEqual(2, items.Count);
            Assert.IsTrue(items.Any(item => item.Text == "Ativo"));
            Assert.IsTrue(items.Any(item => item.Text == "Inativo"));
        }
    }
}
