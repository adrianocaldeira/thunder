using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NUnit.Framework;
using Thunder.Data.Pattern.Repository.Domain;

namespace Thunder.Data.Pattern.Repository
{
    [TestFixture]
    class UserRepositoryAsyncTest
    {
        private readonly DbUtil _dbUtil = new DbUtil();
        private UserRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _dbUtil.Bind();

            using (var session = Thunder.NHibernate.SessionManager.SessionFactory.OpenSession())
            {
                var active = new Status { Name = "Ativo" };
                session.Save(active);

                var inactive = new Status { Name = "Inativo" };
                session.Save(inactive);

                var user = new User { Name = "Adriano", Age = 30, Status = active };
                session.Save(user);
            }

            _repository = new UserRepository();
        }

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Unbind();

            _dbUtil.Clear(typeof(User))
                .Clear(typeof(Status));
        }

        [Test]
        public async Task CreateAsync_e_FindAsync()
        {
            var user = new User { Name = "Assíncrono", Age = 20, Status = new Status { Id = 1 } };

            await _repository.CreateAsync(user);

            var found = await _repository.FindAsync(user.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual("Assíncrono", found.Name);
        }

        [Test]
        public async Task CreateAsync_com_lista()
        {
            await _repository.CreateAsync(new List<User>
            {
                new User { Name = "Lucas", Age = 7, Status = new Status { Id = 1 } },
                new User { Name = "Yasmin", Age = 2, Status = new Status { Id = 1 } }
            });

            var all = await _repository.AllAsync();

            Assert.AreEqual(3, all.Count);
        }

        [Test]
        public async Task UpdateAsync()
        {
            var user = await _repository.FindAsync(1);
            user.Name = "Adriano Caldeira";

            await _repository.UpdateAsync(user);

            Assert.AreEqual("Adriano Caldeira", (await _repository.FindAsync(1)).Name);
        }

        [Test]
        public async Task DeleteAsync_por_id()
        {
            await _repository.DeleteAsync(1);

            Assert.AreEqual(0, (await _repository.AllAsync()).Count);
        }

        [Test]
        public async Task DeleteAsync_com_entidade()
        {
            await _repository.DeleteAsync(await _repository.FindAsync(1));

            Assert.AreEqual(0, (await _repository.AllAsync()).Count);
        }

        [Test]
        public async Task DeleteAsync_com_lista()
        {
            await _repository.DeleteAsync(new List<User> { await _repository.FindAsync(1) });

            Assert.AreEqual(0, (await _repository.AllAsync()).Count);
        }

        [Test]
        public async Task AllAsync_com_expressao()
        {
            Assert.AreEqual(1, (await _repository.AllAsync(x => x.Name == "Adriano" && x.Age >= 30)).Count);
            Assert.AreEqual(0, (await _repository.AllAsync(x => x.Age < 30)).Count);
        }

        [Test]
        public async Task SingleAsync()
        {
            var user1 = await _repository.SingleAsync(x => x.Name == "Adriano" && x.Age >= 30);
            var user2 = await _repository.SingleAsync(x => x.Age < 30);

            Assert.IsNotNull(user1);
            Assert.IsNull(user2);
            Assert.AreEqual("Adriano", user1.Name);
        }

        [Test]
        public async Task ExistAsync()
        {
            Assert.AreEqual(false, await _repository.ExistAsync(1, x => x.Age >= 30));
            Assert.AreEqual(true, await _repository.ExistAsync(0, x => x.Age >= 30));
        }

        [Test]
        public async Task PageAsync_com_orders_e_criterions()
        {
            await _repository.CreateAsync(new List<User>
            {
                new User { Name = "Lucas", Age = 7, Status = new Status { Id = 1 } },
                new User { Name = "Yasmin", Age = 2, Status = new Status { Id = 1 } }
            });

            var porOrdem = await _repository.PageAsync(0, 2, new List<Order> { Order.Desc("Name") });
            Assert.AreEqual(3, porOrdem.Records);
            Assert.AreEqual("Yasmin", porOrdem[0].Name);

            var porCriterion = await _repository.PageAsync(0, 2,
                new List<ICriterion> { Restrictions.Eq("Age", 30), Restrictions.Eq("Name", "Adriano") });
            Assert.AreEqual(1, porCriterion.Records);
            Assert.AreEqual("Adriano", porCriterion[0].Name);
        }
    }
}
