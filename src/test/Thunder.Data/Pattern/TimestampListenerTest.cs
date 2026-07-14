using System;
using NUnit.Framework;
using Thunder.NHibernate;
using Thunder.Data.Pattern.Repository.Domain;

namespace Thunder.Data.Pattern
{
    [TestFixture]
    class TimestampListenerTest
    {
        private readonly DbUtil _dbUtil = new DbUtil();

        [SetUp]
        public void SetUp()
        {
            _dbUtil.Bind();
        }

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Unbind();
            _dbUtil.Clear(typeof(User)).Clear(typeof(Status));
        }

        [Test]
        public void Insert_sobrescreve_created_mesmo_forjado()
        {
            var forjado = new DateTime(2000, 1, 1);
            var status = new Status { Name = "Ativo" };
            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            { s.Save(status); tx.Commit(); }

            var user = new User { Name = "X", Age = 1, Status = status, Created = forjado };
            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            { s.Save(user); tx.Commit(); }

            Assert.Greater(user.Created, forjado);          // servidor sobrescreveu
            Assert.AreEqual(user.Created, user.Updated);    // insert iguala os dois
        }

        [Test]
        public void Update_muda_apenas_updated()
        {
            var status = new Status { Name = "Ativo" };
            var user = new User { Name = "X", Age = 1, Status = status };
            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            { s.Save(status); s.Save(user); tx.Commit(); }
            var created = user.Created;

            System.Threading.Thread.Sleep(15);
            user.Name = "Y";
            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            { s.Update(user); tx.Commit(); }

            Assert.AreEqual(created, user.Created);          // Created intacto
            Assert.Greater(user.Updated, created);           // Updated avançou
        }
    }
}
