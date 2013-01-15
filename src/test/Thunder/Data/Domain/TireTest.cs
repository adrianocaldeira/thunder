using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NUnit.Framework;

namespace Thunder.Data.Domain
{
    [TestFixture]
    class TireTest
    {
        private DbUtil _util;

        [SetUp]
        public void Setup()
        {
            _util = new DbUtil().Bind();
            Tire.Create(new Tire {Name = "Tire 1"});
            Tire.All(Order.Asc("aaa"));
        }

        [TearDown]
        public void TearDown()
        {
            _util.Clear(typeof (Tire)).Unbind();
        }

        [Test]
        public void Create()
        {
            var tire = Tire.Create(new Tire {Name = "Tire 2"});
            Assert.AreEqual(2, tire.Id);
        }

        [Test]
        public void Update()
        {
            var tire = Tire.FindById(1);
            tire.Name = "Tire 2";

            Tire.Update(tire);

            Assert.AreEqual("Tire 2", Tire.FindById(1).Name);
        }

        [Test]
        public void All()
        {
            var tires = Tire.All();

            Assert.AreEqual(1, tires.Count);
            Assert.AreEqual("Tire 1", tires[0].Name);
        }

        [Test]
        public void FindById()
        {
            var tire = Tire.FindById(1);
            Assert.AreEqual(1, tire.Id);
            Assert.AreEqual("Tire 1", tire.Name);
        }

        [Test]
        public void Remove()
        {
            Tire.Delete(1);
            Assert.AreEqual(0, Tire.All().Count);
        }

        [Test]
        public void RemoveByObject()
        {
            Tire.Delete(Tire.FindById(1));
            Assert.AreEqual(0, Tire.All().Count);
        }

        [Test]
        public void AllWithOrder()
        {
            Tire.Create(new Tire {Name = "Tire 2"});
            
            var tires = Tire.All(new Order("Name", false));
            
            Assert.AreEqual(2, tires.Count);
            Assert.AreEqual("Tire 2", tires[0].Name);
            Assert.AreEqual("Tire 1", tires[1].Name);
        }

        [Test]
        public void Where()
        {
            Tire.Create(new Tire { Name = "Tire 2" });

            var tires = Tire.Where(Restrictions.Like("Name", "ir", MatchMode.Anywhere));

            Assert.AreEqual(2, tires.Count);
            Assert.AreEqual("Tire 1", tires[0].Name);
            Assert.AreEqual("Tire 2", tires[1].Name);
        }

        [Test]
        public void Exist()
        {
            Tire.Create(new Tire { Name = "Tire 2" });

            Assert.IsTrue(Tire.Exist(1, "TiRe 2"));
            Assert.IsTrue(Tire.Exist(0, "tire 2"));
            Assert.IsFalse(Tire.Exist(2, "Tire 2"));
            Assert.IsFalse(Tire.Exist(0, "Tire 3"));
        }

        [Test]
        public void Page()
        {
            Tire.Create(new Tire { Name = "Tire 2" });
            Tire.Create(new Tire { Name = "Tire 3" });
            Tire.Create(new Tire { Name = "Tire 4" });
            Tire.Create(new Tire { Name = "Tire 5" });
            Tire.Create(new Tire { Name = "Tire 6" });

            var tires1 = Tire.Page(0, 2);
            var tires2 = Tire.Page(0, 2, new List<ICriterion>{ Restrictions.Like("Name", "ir", MatchMode.Anywhere).IgnoreCase()});
            var tires3 = Tire.Page(0, 2, new List<Order> { Order.Desc("Name") });
            var tires4 = Tire.Page(0, 2, new List<ICriterion> { Restrictions.Like("Name", "ir", MatchMode.Anywhere).IgnoreCase() }, new List<Order> { Order.Desc("Name") });

            Assert.AreEqual(2, tires1.Count);
            Assert.AreEqual(6, tires1.Records);
            Assert.IsTrue(tires1.HasNextPage);
            Assert.IsFalse(tires1.HasPreviousPage);

            Assert.AreEqual(2, tires2.Count);
            Assert.AreEqual(6, tires2.Records);
            Assert.IsTrue(tires2.HasNextPage);
            Assert.IsFalse(tires2.HasPreviousPage);

            Assert.AreEqual(2, tires3.Count);
            Assert.AreEqual(6, tires3.Records);
            Assert.AreEqual("Tire 6", tires3[0].Name);
            Assert.IsTrue(tires3.HasNextPage);
            Assert.IsFalse(tires3.HasPreviousPage);

            Assert.AreEqual(2, tires4.Count);
            Assert.AreEqual(6, tires4.Records);
            Assert.AreEqual("Tire 6", tires4[0].Name);
            Assert.IsTrue(tires4.HasNextPage);
            Assert.IsFalse(tires4.HasPreviousPage);
        }

        [Test]
        public void UpdateProperty()
        {
            Tire.UpdateProperties(1, ActiveRecordProperty<Tire>.Create(x => x.Name, "Tire 4"),
                ActiveRecordProperty<Tire>.Create(x => x.Size, "28"));

            var tire = Tire.FindById(1);

            Assert.AreEqual("Tire 4", tire.Name);
            Assert.AreEqual("28", tire.Size);
        }
    }
}
