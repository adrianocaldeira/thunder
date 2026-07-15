using System.Collections.Generic;
using NUnit.Framework;
using Thunder.Collections;

namespace Thunder.Collections
{
    [TestFixture]
    class PagingLongTest
    {
        [Test]
        public void Records_preserva_valor_maior_que_int_max()
        {
            long total = (long)int.MaxValue + 100;
            var paging = new Paging<int>(new List<int> { 1, 2, 3 }, 0, 10, total);
            Assert.AreEqual(total, paging.Records);
        }
    }
}
