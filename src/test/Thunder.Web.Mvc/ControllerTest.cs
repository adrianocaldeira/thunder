using NUnit.Framework;

namespace Thunder.Web.Mvc
{
    [TestFixture]
    public class ControllerTest
    {
        private class FakeController : Controller
        {
            public JsonResult InvokeSuccess(object data, string contentType)
            {
                return Success(data, contentType);
            }
        }

        [Test]
        public void Success_ComContentType_RespeitaContentTypeInformado()
        {
            var controller = new FakeController();

            var result = controller.InvokeSuccess(new { ok = true }, "text/plain");

            Assert.AreEqual("text/plain", result.ContentType);
        }
    }
}
