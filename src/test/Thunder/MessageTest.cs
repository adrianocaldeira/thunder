using NUnit.Framework;

namespace Thunder
{
    [TestFixture]
    public class MessageTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual("test of message", new Message("message.test").Text);
            Assert.AreEqual("test of message in resource", new Message("message_test",Resource.ResourceManager).Text);
        }
    }
}
