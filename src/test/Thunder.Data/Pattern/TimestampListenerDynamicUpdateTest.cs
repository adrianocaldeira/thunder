using System.Threading;
using NUnit.Framework;
using Thunder.Data.Pattern.Listener.Domain;
using Thunder.NHibernate;

namespace Thunder.Data.Pattern
{
    /// <summary>
    ///     Verifica se o listener de timestamps persiste <c>Updated</c> no banco quando a entidade
    ///     é mapeada com <c>dynamic-update="true"</c> e o update é parcial (entidade attached,
    ///     obtida via <c>Get</c> na própria sessão). Nesse cenário os campos dirty são calculados
    ///     antes do evento <c>PreUpdate</c>; mutar o <c>state</c> no evento pode não incluir a
    ///     coluna no SQL gerado.
    /// </summary>
    [TestFixture]
    internal class TimestampListenerDynamicUpdateTest
    {
        private readonly DbUtil _dbUtil = new DbUtil();

        [TearDown]
        public void TearDown()
        {
            _dbUtil.Clear(typeof(Product));
        }

        [Test]
        [Ignore("Bug confirmado: dynamic-update ignora mutação de state no PreUpdate; correção via IInterceptor é decisão de design pendente")]
        public void Update_parcial_attached_persiste_updated_no_banco()
        {
            // (1) Insert.
            var product = new Product { Name = "Original" };

            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                s.Save(product);
                tx.Commit();
            }

            var updatedAposInsert = product.Updated;

            Thread.Sleep(50);

            // (2) Update parcial attached: Get na própria sessão, altera apenas Name.
            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                var attached = s.Get<Product>(product.Id);
                attached.Name = "Alterado";
                s.SaveOrUpdate(attached);
                tx.Commit();
            }

            // (3) Sessão nova: verifica o que foi de fato PERSISTIDO no banco.
            using (var s = SessionManager.SessionFactory.OpenSession())
            {
                var doBanco = s.Get<Product>(product.Id);

                Assert.AreEqual("Alterado", doBanco.Name, "o update parcial deveria ter persistido Name");
                Assert.Greater(doBanco.Updated, updatedAposInsert,
                    "Updated deveria ter sido persistido no banco pelo update parcial " +
                    "(dynamic-update pode excluir a coluna do SQL quando o state é mutado no PreUpdate)");
            }
        }
    }
}
