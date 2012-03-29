using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Thunder.Security.Domain;

namespace Thunder.NHibernate
{
    [TestFixture]
    public class PagingExtensionsTest
    {
        [Test]
        public void PagingShouldWithEnumerableList()
        {
            var persons = new List<User>().Paging(0, 8);

            Assert.IsNotNull(persons);
        }

        [Test]
        public void PagingShouldWithEnumerableListWithRecords()
        {
            var persons = new List<User>().Paging(0, 8, 30);

            Assert.IsNotNull(persons);
        }

        [Test]
        public void PagingShouldWithQueryableList()
        {
            var persons = new List<User>().AsQueryable().Paging(0, 8);

            Assert.IsNotNull(persons);
        }

        [Test]
        public void PagingShouldWithQueryableListWithRecords()
        {
            var persons = new List<User>().AsQueryable().Paging(0, 8, 30);

            Assert.IsNotNull(persons);
        }
    }
}
