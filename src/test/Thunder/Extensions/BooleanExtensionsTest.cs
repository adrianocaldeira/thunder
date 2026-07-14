using NUnit.Framework;

namespace Thunder.Extensions
{
    [TestFixture]
    public class BooleanExtensionsTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual("Não", false.Text());
            Assert.AreEqual("Sim", true.Text());
        }
    }
}
