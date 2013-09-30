using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NUnit.Framework;
using Thunder.Data.Pattern.ActiveRecord.Domain;

namespace Thunder.Data.Pattern.ActiveRecord
{
    [TestFixture]
    class CarTest
    {
        private readonly DbUtil _dbUtil = new DbUtil();

        [SetUp]
        public void SetUp()
        {
            _dbUtil.Bind();

            using (var session = SessionManager.SessionFactory.OpenSession())
            {
                var active = new Tire {Name = "Pirelli"};
                session.Save(active);

                var inactive = new Tire { Name = "Goodyear" };
                session.Save(inactive);

                var car = new Car {Name = "Gol", Year = 30, Tire = active};
                session.Save(car);
            }
        }

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Unbind();

            _dbUtil.Clear(typeof(Car))
                .Clear(typeof(Tire));
        }

        [Test]
        public void Create()
        {
            Car.Create(new Car { Name = "Prisma", Year = 7, Tire = new Tire { Id = 1 } });

            Assert.AreEqual(2, Car.All().Count);
        }

        [Test]
        public void CreateWithList()
        {
            Car.Create(new List<Car>
                {
                    new Car { Name = "Prisma", Year = 7, Tire = new Tire { Id = 1 } },
                    new Car { Name = "Polo", Year = 2, Tire = new Tire { Id = 1 } }
                });

            Assert.AreEqual(3, Car.All().Count);
        }

        [Test]
        public void Update()
        {
            var user = Car.Find(1);
            user.Name = "Fox";

            Car.Update(user);

            Assert.AreEqual("Fox", Car.Find(1).Name);
        }

        [Test]
        public void UpdateProperty()
        {
            Car.UpdateProperty(1, "Name", "Fox");

            Assert.AreEqual("Fox", Car.Find(1).Name);
        }

        [Test]
        public void UpdatePropertyObject()
        {
            Car.UpdateProperty(1, Property.Create("Name","Fox"));

            Assert.AreEqual("Fox", Car.Find(1).Name);
        }

        [Test]
        public void UpdateProperties()
        {
            Car.UpdateProperties(1, new List<Property<object>>
                {
                    Property.Create("Name", "Fox"),
                    Property.Create("Year", 31),
                    Property.Create("Tire", new Tire{Id = 2})
                });

            var user = Car.Find(1);

            Assert.AreEqual("Fox", user.Name);
            Assert.AreEqual(31, user.Year);
            Assert.AreEqual(new Tire { Id = 2 }, user.Tire);
        }

        [Test]
        public void Delete()
        {
            Car.Delete(1);

            Assert.AreEqual(0, Car.All().Count);
        }

        [Test]
        public void DeleteWithObject()
        {
            Car.Delete(Car.Find(1));

            Assert.AreEqual(0, Car.All().Count);
        }

        [Test]
        public void DeleteWithList()
        {
            Car.Delete(new List<Car> { Car.Find(1) });

            Assert.AreEqual(0, Car.All().Count);
        }

        [Test]
        public void Find()  
        {
            Assert.AreEqual("Gol", Car.Find(1).Name);
        }

        [Test]
        public void All()
        {
            Assert.AreEqual(1, Car.All().Count);
        }

        [Test]
        public void AllWithExpression()
        {
            Assert.AreEqual(1, Car.All(Restrictions.On<Car>(x => x.Name).IsInsensitiveLike("go", MatchMode.Anywhere)).Count);
            Assert.AreEqual(1, Car.All(x => x.Name == "Gol" && x.Year >= 30).Count);
            Assert.AreEqual(0, Car.All(x => x.Year < 30).Count);
        }

        [Test]
        public void Single()
        {
            var user1 = Car.Single(x => x.Name == "Gol" && x.Year >= 30);
            var user2 = Car.Single(x => x.Year < 30);

            Assert.IsNotNull(user1);
            Assert.IsNull(user2);
            Assert.AreEqual("Gol", user1.Name);
        }

        [Test]
        public void Exist()
        {
            Assert.AreEqual(false, Car.Exist(1, x=>x.Year >= 30));
            Assert.AreEqual(false, Car.Exist(1, Restrictions.On<Car>(x => x.Name).IsInsensitiveLike("go", MatchMode.Anywhere)));
            Assert.AreEqual(true, Car.Exist(0, x => x.Year >= 30));
            Assert.AreEqual(true, Car.Exist(0, Restrictions.On<Car>(x => x.Name).IsInsensitiveLike("go", MatchMode.Anywhere)));
        }

        [Test]
        public void Page()
        {
            Car.Create(new List<Car>
                {
                    new Car { Name = "Prisma", Year = 7, Tire = new Tire { Id = 1 } },
                    new Car { Name = "Polo", Year = 2, Tire = new Tire { Id = 1 } }
                });

            Assert.AreEqual(3, Car.Page(0, 2).Records);
            Assert.AreEqual("Prisma", Car.Page(0, 2, Order.Desc("Name"))[0].Name);
            Assert.AreEqual("Polo", Car.Page(0, 2, new List<Order> { Order.Asc("Year"), Order.Asc("Name") })[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, x => x.Year == 30 )[0].Name);
            Assert.AreEqual("Prisma", Car.Page(0, 2, x => x.Year < 30, Order.Desc("Year"))[0].Name);
            Assert.AreEqual("Polo", Car.Page(0, 2, new List<Expression<Func<Car, bool>>>{x=> x.Year == 2 && x.Name == "Polo"})[0].Name);
            Assert.AreEqual("Polo", Car.Page(0, 2, new List<Expression<Func<Car, bool>>> { x => x.Year < 30 }, Order.Asc("Name"))[0].Name);
            Assert.AreEqual("Polo", Car.Page(0, 2, new List<Expression<Func<Car, bool>>> { x => x.Year < 30 }, new List<Order> { Order.Asc("Year") })[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, Restrictions.Eq("Year", 30))[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, Restrictions.Eq("Year", 30), Order.Asc("Name"))[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, Restrictions.Eq("Year", 30), new List<Order> { Order.Asc("Name") })[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, new List<ICriterion> { Restrictions.Eq("Year", 30), Restrictions.Eq("Name", "Gol") })[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, new List<ICriterion> { Restrictions.Eq("Year", 30), Restrictions.Eq("Name", "Gol") }, Order.Asc("Name"))[0].Name);
            Assert.AreEqual("Gol", Car.Page(0, 2, new List<ICriterion> { Restrictions.Eq("Year", 30), Restrictions.Eq("Name", "Gol") }, new List<Order> { Order.Asc("Name") })[0].Name);
        }
    }
}
