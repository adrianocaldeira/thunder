using System;
using System.IO;
using NHibernate.Cfg;
using NUnit.Framework;
using Thunder.NHibernate;

namespace Thunder.Data
{
#pragma warning disable 618
    [TestFixture]
    public class CfgSerializationTest
    {
        private CfgSerialization _serialization;

        [SetUp]
        public void Setup()
        {
            _serialization = new CfgSerialization("cfg.oca");
        }

        [TearDown]
        public void TearDown()
        {
            _serialization.Delete();
        }

        [Test]
        public void ShouldGenerateException()
        {
            var ex = Assert.Throws<ArgumentException>(() => new CfgSerialization(null));
            StringAssert.Contains("File name no information.", ex.Message);
        }

        [Test]
        public void ShouldThrowNotSupportedExceptionOnCreate()
        {
            var ex = Assert.Throws<NotSupportedException>(() => _serialization.Create(new Configuration().Configure()));
            StringAssert.Contains("CWE-502", ex.Message);
        }

        [Test]
        public void ShouldThrowNotSupportedExceptionOnLoad()
        {
            var ex = Assert.Throws<NotSupportedException>(() => _serialization.Load());
            StringAssert.Contains("CWE-502", ex.Message);
        }

        [Test]
        public void ShouldThrowNotSupportedExceptionOnLoadWithFileName()
        {
            var ex = Assert.Throws<NotSupportedException>(() => _serialization.Load("cfg.thunder"));
            StringAssert.Contains("CWE-502", ex.Message);
        }

        [Test]
        public void ShouldNotCreateFileOnDisk()
        {
            Assert.Throws<NotSupportedException>(() => _serialization.Create(new Configuration().Configure()));

            Assert.False(File.Exists(_serialization.GetPath()));
        }

        [Test]
        public void ShouldBeMarkedObsolete()
        {
            var attributes = typeof(CfgSerialization).GetCustomAttributes(typeof(ObsoleteAttribute), false);

            Assert.IsNotEmpty(attributes);
        }
    }
#pragma warning restore 618
}
