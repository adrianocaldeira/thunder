using NUnit.Framework;
using Thunder.Resources;

namespace Thunder.Web
{
    [TestFixture]
    class JsonExtensionsTest
    {
        [Test]
        public void SerializeObject()
        {
            var json = new TireItem {Id = 1, State = ObjectState.Unchanged}.Json();
            Assert.AreEqual("{\"Id\":1,\"State\":3}", json);
        }

        [Test]
        public void DeserializeObject()
        {
            var tireItem = "{\"Id\":1,\"State\":3}".Json<TireItem>();
            Assert.AreEqual(1, tireItem.Id);
            Assert.AreEqual(ObjectState.Unchanged, tireItem.State);
        }
    }
}
