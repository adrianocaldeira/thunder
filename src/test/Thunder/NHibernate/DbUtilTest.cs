using NUnit.Framework;
using Thunder.NHibernate.Domain;

namespace Thunder.NHibernate
{
    [TestFixture]
    public class DbUtilTest
    {
        [Test]
        public void Clear()
        {
            var dbUtil = new DbUtil(SessionManager.Configuration());
            dbUtil.Create();

            var session = SessionManager.OpenSession();
            using (var transaction = session.BeginTransaction())
            {
                session.Save(new Tire { Id = 1, Name = "Pirelli" });
                session.Save(new Tire { Id = 2, Name = "Goodyear" });

                session.Save(new Gol { Color = "Black", Door = 4, Name = "Gol 1.0 Flex", Tire = new Tire { Id = 1 } });
                session.Save(new Gol { Color = "White", Door = 4, Name = "Gol 1.6 Flex", Tire = new Tire { Id = 1 } });
                session.Save(new Uno { Color = "Red", Door = 2, Name = "Uno 1.0", Tire = new Tire { Id = 2 } });

                transaction.Commit();
            }

            dbUtil.Clear(typeof(Tire)).Clear(typeof(Gol))
                .Clear(typeof(Uno)).Clear(typeof(Car));            
        }
    }
}
