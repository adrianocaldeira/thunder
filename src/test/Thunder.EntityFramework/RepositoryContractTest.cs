using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using Moq;
using NUnit.Framework;
using Thunder.Data;
using Thunder.EntityFramework.Pattern;

// Permite ao Moq/Castle criar proxies dos tipos internos de teste (Foo).
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Thunder.EntityFramework
{
    class Foo : Persist<Foo, int> { }
    class FooRepository : Repository<Foo, int>
    {
        public FooRepository(DbContext c) : base(c) { }
    }

    [TestFixture]
    class RepositoryContractTest
    {
        private static (Mock<DbContext> ctx, Mock<DbSet<Foo>> set) Mocks()
        {
            var set = new Mock<DbSet<Foo>>();
            var ctx = new Mock<DbContext>();
            ctx.Setup(c => c.Set<Foo>()).Returns(set.Object);
            return (ctx, set);
        }

        [Test]
        public void Add_nao_comita()
        {
            var (ctx, _) = Mocks();
            new FooRepository(ctx.Object).Add(new Foo());
            ctx.Verify(c => c.SaveChanges(), Times.Never);
        }

        [Test]
        public void Save_comita()
        {
            var (ctx, _) = Mocks();
            new FooRepository(ctx.Object).Save();
            ctx.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Test]
        public void Nao_e_disposable()
        {
            Assert.IsFalse(typeof(Repository<Foo, int>).GetInterfaces()
                .Contains(typeof(IDisposable)));
        }
    }
}
