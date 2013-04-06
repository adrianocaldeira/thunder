using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Thunder.Extensions;

namespace Thunder
{
    [TestFixture]
    public class ValidatorExtensionsTest
    {
        [Test]
        public void Test()
        {
            Assert.IsTrue("46.172.851/0001-12".IsCnpj());
            Assert.IsTrue("420.754.226-48".IsCpf());
            Assert.IsTrue("28/05/1983".IsDate());
            Assert.IsTrue("29/02/2012".IsDate());
            Assert.IsTrue("adriano@integgro.com.br".IsEmail());
            Assert.IsTrue("adriano@integgro.com".IsEmail());
            Assert.IsTrue("23:59".IsHour());

            Assert.IsFalse("46.172.851/0002-12".IsCnpj());
            Assert.IsFalse("00.000.000/0000-00".IsCnpj());

            Assert.IsFalse("120.754.226-48".IsCpf());
            Assert.IsFalse("000.000.000-00".IsCpf());

            Assert.IsFalse("30/02/2012".IsDate());
            Assert.IsFalse("00/02/2012".IsDate());
            Assert.IsFalse("32/02/2012".IsDate());
            Assert.IsFalse("01/00/2012".IsDate());
            Assert.IsFalse("01/13/2012".IsDate());
            Assert.IsFalse("31/04/2012".IsDate());
            Assert.IsFalse("31/06/2012".IsDate());
            Assert.IsFalse("31/09/2012".IsDate());
            Assert.IsFalse("31/11/2012".IsDate());

            Assert.IsFalse("adrianointeggro.com.br".IsEmail());
            Assert.IsFalse("adriano@integgro".IsEmail());
            Assert.IsFalse("@integgro.com".IsEmail());

            Assert.IsFalse("24:60".IsHour());
        }
    }
}
