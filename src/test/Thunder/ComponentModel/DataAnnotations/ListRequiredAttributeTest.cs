using System.Collections.Generic;
using NUnit.Framework;
using Thunder.Security.Domain;

namespace Thunder.ComponentModel.DataAnnotations
{
    [TestFixture]
    class ListRequiredAttributeTest
    {
        [Test]
        public void Test()
        {
            var requiredAttribute = new ListRequiredAttribute();

            Assert.IsFalse(requiredAttribute.IsValid(null));
            Assert.IsFalse(requiredAttribute.IsValid(new List<string>()));
            Assert.IsFalse(requiredAttribute.IsValid(new List<User>()));
            Assert.IsTrue(requiredAttribute.IsValid(new List<string> {"a", "b"}));
            Assert.IsTrue(requiredAttribute.IsValid(new List<User> { new User(), new User() }));
        }
    }
}
