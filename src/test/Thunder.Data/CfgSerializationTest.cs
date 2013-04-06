using System;
using System.IO;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Thunder.Data
{
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
        [ExpectedException(typeof(ArgumentException), ExpectedMessage = "File name no information.", MatchType = MessageMatch.Contains)]
        public void ShouldGenerateException()
        {
            new CfgSerialization(null);
        }
        
        [Test]
        public void ShouldCreate()
        {
            _serialization.Create(new Configuration().Configure());

            Assert.True(File.Exists(_serialization.GetPath()));
        }

        [Test]
        public void ShouldDelete()
        {
            _serialization.Create(new Configuration().Configure());
            _serialization.Delete();

            Assert.False(File.Exists(_serialization.GetPath()));
        }

        [Test]
        public void ShouldLoad()
        {
            _serialization.Create(new Configuration().Configure());
            var configuration = _serialization.Load();

            Assert.IsNotNull(configuration);
        }

        [Test]
        public void ShouldLoadForceCreate()
        {
            var configuration = _serialization.Load();

            Assert.IsNotNull(configuration);
        }
    }
}