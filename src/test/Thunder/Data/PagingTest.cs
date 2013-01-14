using System;
using System.Linq;
using NUnit.Framework;
using Thunder.Collections;

namespace Thunder.Data
{
    [TestFixture]
    public class PagingTest
    {
        private System.Collections.Generic.List<string> _persons;
        private int _pageSize;

        [SetUp]
        public void SetUp()
        {
            _persons = new System.Collections.Generic.List<string>();

            for (var i = 0; i < 30; i++)
            {
                _persons.Add(i.ToString());
            }

            _pageSize = 8;
        }

        [Test]
        public void PagingShouldWithEnumerableList()
        {
            const int currentPage = 0;
            const int records = 30;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.AreEqual(currentPage, paging.CurrentPage);
            Assert.AreEqual(8, paging.Count);
            Assert.AreEqual(4, paging.PageCount);
            Assert.AreEqual(_pageSize, paging.PageSize);
            Assert.AreEqual(records, (int) paging.Records);
            Assert.AreEqual(0, paging.Skip);
            Assert.IsTrue(paging.HasNextPage);
            Assert.IsFalse(paging.HasPreviousPage);
            Assert.IsTrue(paging.IsFirstPage);
            Assert.IsFalse(paging.IsLastPage);
        }

        [Test]
        public void PagingShouldWithQueryableList()
        {
            const int currentPage = 0;
            const int records = 30;

            var paging = new Paging<string>(_persons.AsQueryable(), currentPage, _pageSize);

            Assert.AreEqual(currentPage, paging.CurrentPage);
            Assert.AreEqual(8, paging.Count);
            Assert.AreEqual(4, paging.PageCount);
            Assert.AreEqual(_pageSize, paging.PageSize);
            Assert.AreEqual(records, (int) paging.Records);
            Assert.AreEqual(0, paging.Skip);
        }

        [Test]
        public void PagingShouldWithRecords()
        {
            const int currentPage = 1;
            const int records = 30;

            var persons = new System.Collections.Generic.List<string>();

            for (var i = 0; i < 5; i++)
            {
                persons.Add(i.ToString());
            }

            var paging = new Paging<string>(persons, currentPage, _pageSize, 30);

            Assert.AreEqual(currentPage, paging.CurrentPage);
            Assert.AreEqual(5, paging.Count);
            Assert.AreEqual(4, paging.PageCount);
            Assert.AreEqual(_pageSize, paging.PageSize);
            Assert.AreEqual(records, (int) paging.Records);
            Assert.AreEqual(0, paging.Skip);
        }

        [Test]
        public void PagingShouldEmpty()
        {
            const int currentPage = 1;

            var paging = new Paging<string>(new System.Collections.Generic.List<string>(), currentPage, _pageSize);

            Assert.AreEqual(currentPage, paging.CurrentPage);
            Assert.AreEqual(0, paging.Count);
            Assert.AreEqual(0, paging.PageCount);
            Assert.AreEqual(_pageSize, paging.PageSize);
            Assert.AreEqual(0, paging.Skip);
        }

        [Test]
        public void PagingShouldNullSource()
        {
            const int currentPage = 1;

            var paging = new Paging<string>(null, currentPage, _pageSize);

            Assert.AreEqual(currentPage, paging.CurrentPage);
            Assert.AreEqual(0, paging.Count);
            Assert.AreEqual(0, paging.PageCount);
            Assert.AreEqual(_pageSize, paging.PageSize);
            Assert.AreEqual(0, paging.Skip);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidCurrentPageArgument()
        {
            new Paging<string>(new System.Collections.Generic.List<string>(), -1, _pageSize);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ValidPageSizeArgument()
        {
            new Paging<string>(new System.Collections.Generic.List<string>(), 0, -1);
        }

        [Test]
        public void ShouldFirstPage()
        {
            const int currentPage = 0;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsTrue(paging.IsFirstPage);
        }

        [Test]
        public void ShouldNotFirstPage()
        {
            const int currentPage = 1;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsFalse(paging.IsFirstPage);
        }

        [Test]
        public void ShouldLastPage()
        {
            const int currentPage = 3;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsTrue(paging.IsLastPage);
        }

        [Test]
        public void ShouldNotLastPage()
        {
            const int currentPage = 2;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsFalse(paging.IsLastPage);
        }

        [Test]
        public void ShouldHasNextPage()
        {
            const int currentPage = 0;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsTrue(paging.HasNextPage);
        }

        [Test]
        public void ShouldNotHasNextPage()
        {
            const int currentPage = 3;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsFalse(paging.HasNextPage);
        }

        [Test]
        public void ShouldHasPreviousPage()
        {
            const int currentPage = 3;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsTrue(paging.HasPreviousPage);
        }

        [Test]
        public void ShouldNotHasPreviousPage()
        {
            const int currentPage = 0;

            var paging = new Paging<string>(_persons, currentPage, _pageSize);

            Assert.IsFalse(paging.HasPreviousPage);
        }
    }
}
