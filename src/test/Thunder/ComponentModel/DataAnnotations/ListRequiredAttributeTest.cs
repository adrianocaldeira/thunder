using System.Collections.Generic;
using NUnit.Framework;

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
            Assert.IsTrue(requiredAttribute.IsValid(new List<string> {"a", "b"}));
        }
    }
}
