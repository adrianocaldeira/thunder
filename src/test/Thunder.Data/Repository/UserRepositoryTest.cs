using System.Collections.Generic;
using NUnit.Framework;
using Thunder.Data.Domain;
using log4net.Config;
using System.Linq;
using System.Linq.Expressions;

namespace Thunder.Data.Repository
{
    [TestFixture]
    class UserRepositoryTest
    {
        private IUserRepository _repository;
        private DbUtil _dbUtil;

        [SetUp]
        public void SetUp()
        {
            XmlConfigurator.Configure();

            _dbUtil = new DbUtil().Bind();
            _repository = new UserRepository();

            using (var transaction = _repository.Session.BeginTransaction())
            {
                _repository.Session.Save(new User { Id = 1, Age = 30, Name = "Adriano Caldeira" });

                transaction.Commit();
            }
        }

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Unbind().Clear(typeof(User));
        }

        [Test]
        public void Create()
        {
            _repository.Create(new User {Age = 28, Name = "Marcos"});

            Assert.AreEqual(2, _repository.All().Count());
        }

        [Test]
        public void CreateFromList()
        {
            _repository.Create(new List<User>
                {
                    new User { Age = 28, Name = "Marcos" },
                    new User { Age = 22, Name = "Fabio" }
                });

            Assert.AreEqual(3, _repository.All().Count());
        }

        [Test]
        public void Update()
        {
            var user = _repository.FindById(1);
            user.Name = "Adriano H Caldeira";
            user.Age = 31;

            _repository.Update(user);

            Assert.AreEqual("Adriano H Caldeira", _repository.FindById(1).Name);
        }

        [Test]
        public void Delete()
        {
            _repository.Delete(1);
            Assert.AreEqual(0, _repository.All().Count());
        }

        [Test]
        public void DeleteFromObject()
        {
            _repository.Delete(_repository.FindById(1));
            Assert.AreEqual(0, _repository.All().Count());
        }

        [Test]
        public void DeleteFromList()
        {
            _repository.Delete(new List<User> { _repository.FindById(1) });
            Assert.AreEqual(0, _repository.All().Count());
        }

        [Test]
        public void FindById()
        {
            Assert.AreEqual("Adriano Caldeira", _repository.FindById(1).Name);
        }

        [Test]
        public void All()
        {
            Assert.AreEqual(1, _repository.All().Count());
        }

        [Test]
        public void Single()
        {
            Assert.AreEqual("Adriano Caldeira", _repository.Single(x => x.Id == 1).Name);
        }

        [Test]
        public void Find()
        {
            Assert.AreEqual(1, _repository.Find(x => x.Name.Contains("caldei")).Count());
        }
    }
}