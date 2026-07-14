using NUnit.Framework;

namespace Thunder.Web.Mvc.Internal
{
    [TestFixture]
    public class JsonpCallbackTest
    {
        [TestCase("foo")]
        [TestCase("ns.foo")]
        [TestCase("a_$1")]
        [TestCase("obj.method")]
        public void IsValid_NomeSeguro_RetornaTrue(string callback)
        {
            Assert.IsTrue(JsonpCallback.IsValid(callback));
        }

        [TestCase("alert(1)")]
        [TestCase("a-b")]
        [TestCase("")]
        [TestCase("<x>")]
        [TestCase("a b")]
        [TestCase("1foo")]
        public void IsValid_NomeInseguroOuInvalido_RetornaFalse(string callback)
        {
            Assert.IsFalse(JsonpCallback.IsValid(callback));
        }

        [Test]
        public void IsValid_Null_RetornaFalse()
        {
            Assert.IsFalse(JsonpCallback.IsValid(null));
        }
    }
}
