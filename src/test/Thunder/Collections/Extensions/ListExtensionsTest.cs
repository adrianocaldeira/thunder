using System.Collections.Generic;
using NUnit.Framework;
using Thunder.Resources;

namespace Thunder.Collections.Extensions
{
    [TestFixture]
    internal class ListExtensionsTest
    {
        [Test]
        public void In()
        {
            var items = new List<TireItem>
                {
                    new TireItem {State = ObjectState.Unchanged},
                    new TireItem {State = ObjectState.Added},
                    new TireItem {State = ObjectState.Deleted}
                };

            Assert.AreEqual(3, items.In(ObjectState.Added, ObjectState.Deleted, ObjectState.Unchanged).Count);
            Assert.AreEqual(1, items.In(ObjectState.Unchanged).Count);
            Assert.AreEqual(1, items.In(ObjectState.Deleted).Count);
            Assert.AreEqual(1, items.In(ObjectState.Added).Count);
            Assert.AreEqual(0, items.In(ObjectState.Modified).Count);
        }

        [Test]
        public void NotIn()
        {
            var items = new List<TireItem>
                {
                    new TireItem {State = ObjectState.Unchanged},
                    new TireItem {State = ObjectState.Added},
                    new TireItem {State = ObjectState.Deleted}
                };

            Assert.AreEqual(0, items.NotIn(ObjectState.Added, ObjectState.Deleted, ObjectState.Unchanged).Count);
            Assert.AreEqual(2, items.NotIn(ObjectState.Unchanged).Count);
            Assert.AreEqual(2, items.NotIn(ObjectState.Deleted).Count);
            Assert.AreEqual(2, items.NotIn(ObjectState.Added).Count);
            Assert.AreEqual(3, items.NotIn(ObjectState.Modified).Count);
        }

        [Test]
        public void Reorganize()
        {
            var items = new List<TireItem>
                {
                    new TireItem(),
                    new TireItem(),
                    new TireItem()
                };

            items.Reorganize(x => x.Id);

            Assert.AreEqual(0,items[0].Id);
            Assert.AreEqual(1, items[1].Id);
            Assert.AreEqual(2, items[2].Id);
        }
    }
}