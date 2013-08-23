using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NUnit.Framework;
using Thunder.Collections;
using Thunder.Data.Pattern.Repository.Domain;

namespace Thunder.Data.Pattern.Repository
{
    [TestFixture]
    class UserRepositoryTest
    {
        private readonly DbUtil _dbUtil = new DbUtil();
        private IUserRepository Repository;

        [SetUp]
        public void SetUp()
        {
            _dbUtil.Bind();

            using (var session = SessionManager.SessionFactory.OpenSession())
            {
                var active = new Status {Name = "Ativo"};
                active.NotifyCreated();
                active.NotifyUpdated();
                session.Save(active);

                var inactive = new Status { Name = "Inativo" };
                inactive.NotifyCreated();
                inactive.NotifyUpdated();
                session.Save(inactive);

                var user = new User {Name = "Adriano", Age = 30, Status = active};
                user.NotifyCreated();
                user.NotifyUpdated();

                session.Save(user);
            }

            Repository = new UserRepository();
        }

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Unbind();

            _dbUtil.Clear(typeof(User))
                .Clear(typeof(Status));
        }

        [Test]
        public void Create()
        {
            Repository.Create(new User { Name = "Lucas", Age = 7, Status = new Status { Id = 1 } });

            Assert.AreEqual(2, Repository.All().Count);
        }

        [Test]
        public void CreateWithList()
        {
            Repository.Create(new List<User>
                {
                    new User { Name = "Lucas", Age = 7, Status = new Status { Id = 1 } },
                    new User { Name = "Yasmin", Age = 2, Status = new Status { Id = 1 } }
                });

            Assert.AreEqual(3, Repository.All().Count);
        }

        [Test]
        public void Update()
        {
            var user = Repository.Find(1);
            user.Name = "Adriano Caldeira";

            Repository.Update(user);

            Assert.AreEqual("Adriano Caldeira", Repository.Find(1).Name);
        }

        [Test]
        public void UpdateProperty()
        {
            Repository.UpdateProperty(1, "Name", "Adriano Caldeira");

            Assert.AreEqual("Adriano Caldeira", Repository.Find(1).Name);
        }

        [Test]
        public void UpdatePropertyObject()
        {
            Repository.UpdateProperty(1, Property.Create("Name","Adriano Caldeira"));

            Assert.AreEqual("Adriano Caldeira", Repository.Find(1).Name);
        }

        [Test]
        public void UpdateProperties()
        {
            Repository.UpdateProperties(1, new List<Property<object>>
                {
                    Property.Create("Name", "Adriano Caldeira"),
                    Property.Create("Age", 31),
                    Property.Create("Status", new Status{Id = 2})
                });

            var user = Repository.Find(1);

            Assert.AreEqual("Adriano Caldeira", user.Name);
            Assert.AreEqual(31, user.Age);
            Assert.AreEqual(new Status { Id = 2 }, user.Status);
        }

        [Test]
        public void Delete()
        {
            Repository.Delete(1);

            Assert.AreEqual(0, Repository.All().Count);
        }

        [Test]
        public void DeleteWithObject()
        {
            Repository.Delete(Repository.Find(1));

            Assert.AreEqual(0, Repository.All().Count);
        }

        [Test]
        public void DeleteWithList()
        {
            Repository.Delete(new List<User> { Repository.Find(1) });

            Assert.AreEqual(0, Repository.All().Count);
        }

        [Test]
        public void Find()  
        {
            Assert.AreEqual("Adriano", Repository.Find(1).Name);
        }

        [Test]
        public void All()
        {
            Assert.AreEqual(1, Repository.All().Count);
        }

        [Test]
        public void AllWithExpression()
        {
            Assert.AreEqual(1, Repository.All(Restrictions.On<User>(x => x.Name).IsInsensitiveLike("adr", MatchMode.Anywhere)).Count);
            Assert.AreEqual(1, Repository.All(x => x.Name == "Adriano" && x.Age >= 30).Count);
            Assert.AreEqual(0, Repository.All(x => x.Age < 30).Count);
        }

        [Test]
        public void Single()
        {
            var user1 = Repository.Single(x => x.Name == "Adriano" && x.Age >= 30);
            var user2 = Repository.Single(x => x.Age < 30);

            Assert.IsNotNull(user1);
            Assert.IsNull(user2);
            Assert.AreEqual("Adriano", user1.Name);
        }

        [Test]
        public void Exist()
        {
            Assert.AreEqual(false, Repository.Exist(1, x=>x.Age >= 30));
            Assert.AreEqual(false, Repository.Exist(1, Restrictions.On<User>(x => x.Name).IsInsensitiveLike("adr", MatchMode.Anywhere)));
            Assert.AreEqual(true, Repository.Exist(0, x => x.Age >= 30));
            Assert.AreEqual(true, Repository.Exist(0, Restrictions.On<User>(x => x.Name).IsInsensitiveLike("adr", MatchMode.Anywhere)));
        }

        [Test]
        public void Page()
        {
            Repository.Create(new List<User>
                {
                    new User { Name = "Lucas", Age = 7, Status = new Status { Id = 1 } },
                    new User { Name = "Yasmin", Age = 2, Status = new Status { Id = 1 } }
                });

            
            Assert.AreEqual(3, Repository.Page(0, 2).Records);
            Assert.AreEqual("Yasmin", Repository.Page(0, 2, Order.Desc("Name"))[0].Name);
            Assert.AreEqual("Yasmin", Repository.Page(0, 2, new List<Order> { Order.Asc("Age"), Order.Asc("Name") })[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, x => x.Age == 30 )[0].Name);
            Assert.AreEqual("Lucas", Repository.Page(0, 2, x => x.Age < 30, Order.Desc("Age"))[0].Name);
            Assert.AreEqual("Yasmin", Repository.Page(0, 2, new List<Expression<Func<User, bool>>>{x=> x.Age == 2 && x.Name == "Yasmin"})[0].Name);
            Assert.AreEqual("Lucas", Repository.Page(0, 2, new List<Expression<Func<User, bool>>> { x => x.Age < 30 }, Order.Asc("Name"))[0].Name);
            Assert.AreEqual("Yasmin", Repository.Page(0, 2, new List<Expression<Func<User, bool>>> { x => x.Age < 30 }, new List<Order> { Order.Asc("Age") })[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, Restrictions.Eq("Age", 30))[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, Restrictions.Eq("Age", 30), Order.Asc("Name"))[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, Restrictions.Eq("Age", 30), new List<Order> { Order.Asc("Name") })[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, new List<ICriterion> { Restrictions.Eq("Age", 30), Restrictions.Eq("Name", "Adriano") })[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, new List<ICriterion> { Restrictions.Eq("Age", 30), Restrictions.Eq("Name", "Adriano") }, Order.Asc("Name"))[0].Name);
            Assert.AreEqual("Adriano", Repository.Page(0, 2, new List<ICriterion> { Restrictions.Eq("Age", 30), Restrictions.Eq("Name", "Adriano") }, new List<Order> { Order.Asc("Name") })[0].Name);
        }
    }
}
