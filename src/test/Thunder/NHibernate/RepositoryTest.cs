using NHibernate;
using NUnit.Framework;
using Thunder.Security.Domain;

namespace Thunder.NHibernate
{
    [TestFixture]
    public class RepositoryTest
    {
        private Repository<User, int> _repository;
        private DbUtil _dbUtil;
        private ISession _session;

        [SetUp]
        public void Setup()
        {
            _session = SessionManager.OpenSession();

            _dbUtil = new DbUtil(SessionManager.Configuration());
            _dbUtil.Create();

            _repository = new Repository<User, int>(_session);
            _repository.Add(new User { Name = "Adriano", Password = "123", Active = true, Login = "adriano"});
        }

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Close(_session).Clear(typeof(User));
        }

        [Test]
        public void Delete()
        {
            _repository.Remove(1);

            Assert.AreEqual(0, _repository.FindAll().Count);
        }

        [Test]
        public void DeleteByEntity()
        {
            _repository.Remove(_repository.FindById(1));

            Assert.AreEqual(0, _repository.FindAll().Count);
        }

        [Test]
        public void Get()
        {
            var person = _repository.FindById(1);

            Assert.AreEqual("Adriano", person.Name);
        }

        [Test]
        public void NoGet()
        {
            var person = _repository.FindById(2);

            Assert.IsNull(person);
        }

        [Test]
        public void List()
        {
            Assert.AreEqual(1, _repository.FindAll().Count);
        }

        [Test]
        public void Save()
        {
            _repository.Add(new User { Name = "Lucas", Password = "123", Active = true, Login = "lucas" });

            Assert.AreEqual(2, _repository.FindAll().Count);
        }

        [Test]
        public void Update()
        {
            var person = _repository.FindById(1);
            person.Name = "Lucas";

            _repository.Update(person);

            Assert.AreEqual("Lucas", _repository.FindById(1).Name);
        }

        [Test]
        public void GetSession()
        {
            Assert.IsNotNull(_repository.Session);
        }
    }
}
