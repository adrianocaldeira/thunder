using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Thunder.Data.Pattern.Listener.Domain;
using Thunder.NHibernate;
using Thunder.NHibernate.Pattern;

namespace Thunder.Data.Pattern
{
    /// <summary>
    ///     Verifica a persistência dos timestamps em entidades mapeadas com <c>dynamic-update="true"</c>,
    ///     nas quais o UPDATE contém apenas as colunas dirty (calculadas antes do evento <c>PreUpdate</c>).
    ///     A consistência é garantida pelo <see cref="CreatedAndUpdatedFlushEntityListener" /> (registrado
    ///     em <see cref="TestEnvironment" /> via <c>ListenerType.FlushEntity</c>), que define <c>Updated</c>
    ///     antes do dirty-check padrão — incluindo a coluna no UPDATE dinâmico. As comparações são feitas
    ///     banco-a-banco (valores relidos em sessão nova) para eliminar ruído de precisão do SQLite.
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

            var updatedAposInsert = LerDoBanco(product.Id).Updated;

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
            var doBanco = LerDoBanco(product.Id);

            Assert.AreEqual("Alterado", doBanco.Name, "o update parcial deveria ter persistido Name");
            Assert.Greater(doBanco.Updated, updatedAposInsert,
                "Updated deveria ter sido persistido no banco pelo update parcial " +
                "(o listener de FlushEntity inclui a coluna nos campos dirty do UPDATE dinâmico)");
        }

        [Test]
        public void Insert_persiste_created_e_updated_no_banco()
        {
            var product = new Product { Name = "Novo" };

            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                s.Save(product);
                tx.Commit();
            }

            // Sessão nova: o INSERT (coberto pelo PreInsert) deve ter persistido os dois timestamps.
            var doBanco = LerDoBanco(product.Id);

            Assert.AreNotEqual(default(DateTime), doBanco.Created, "Created deveria ter sido persistido no insert");
            Assert.AreNotEqual(default(DateTime), doBanco.Updated, "Updated deveria ter sido persistido no insert");
            Assert.AreEqual(doBanco.Created, doBanco.Updated, "no insert, Created e Updated devem ser iguais");
        }

        [Test]
        public void Flush_sem_alteracao_nao_gera_update_espurio()
        {
            var product = new Product { Name = "Estável" };

            using (var s = SessionManager.SessionFactory.OpenSession())
            using (var tx = s.BeginTransaction())
            {
                s.Save(product);
                tx.Commit();
            }

            var updatedAposInsert = LerDoBanco(product.Id).Updated;

            Thread.Sleep(50);

            // Get + commit sem alterar nada: a guarda de dirty do listener não pode marcar a
            // entidade como alterada — nenhum UPDATE deve ser emitido (SQL capturado do show_sql).
            var consoleOriginal = Console.Out;
            var sqlCapturado = new StringWriter();

            try
            {
                Console.SetOut(sqlCapturado);

                using (var s = SessionManager.SessionFactory.OpenSession())
                using (var tx = s.BeginTransaction())
                {
                    s.Get<Product>(product.Id);
                    tx.Commit();
                }
            }
            finally
            {
                Console.SetOut(consoleOriginal);
            }

            StringAssert.DoesNotContain("UPDATE", sqlCapturado.ToString(),
                "flush sem alteração não deveria emitir UPDATE (o listener não pode sujar a entidade)");

            Assert.AreEqual(updatedAposInsert, LerDoBanco(product.Id).Updated,
                "Updated no banco não deveria mudar num flush sem alteração");
        }

        private static Product LerDoBanco(int id)
        {
            using (var s = SessionManager.SessionFactory.OpenSession())
            {
                return s.Get<Product>(id);
            }
        }
    }
}
