using System;
using System.Web.Mvc;
using NUnit.Framework;
using Thunder.Mock;
using Thunder.Web.Mvc.Html.Design;

namespace Thunder.Web.Mvc.Html
{
    [TestFixture]
    public class ImageExtensionsTest
    {
        [Test]
        public void Test()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            Assert.AreEqual("<img border=\"0\" src=\"teste.jpg\" />", htmlHelper.Image("teste.jpg").ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.jpg\" style=\"position: absolute;\" />", htmlHelper.Image("teste.jpg", new { style = "position: absolute;" }).ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.jpg\" />", htmlHelper.Jpg("teste.jpg").ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.jpg\" />", htmlHelper.Jpg("teste").ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.png\" />", htmlHelper.Png("teste.png").ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.png\" />", htmlHelper.Png("teste").ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.gIf\" />", htmlHelper.Gif("teste.gIf").ToHtmlString());
            Assert.AreEqual("<img border=\"0\" src=\"teste.gif\" />", htmlHelper.Gif("teste").ToHtmlString());
        }

        [Test, ExpectedException(ExpectedException = typeof(ArgumentNullException))]
        public void TestNull()
        {
            var htmlHelper = MockHtmlHelper.Make(new ViewDataDictionary());

            htmlHelper.Image(null);
            htmlHelper.Image(null, new {style = "position: absolute;"});
            htmlHelper.Png(null);
            htmlHelper.Gif(null);
            htmlHelper.Jpg(null);
        }
    }
}
