using NUnit.Framework;

namespace Thunder.ComponentModel.DataAnnotations
{
    [TestFixture]
    internal class BooleanRequiredAttributeTest
    {
        [Test]
        public void Valid()
        {
            var attribute = new BooleanRequiredAttribute();

            Assert.IsFalse(attribute.IsValid(""));
            Assert.IsFalse(attribute.IsValid("Select"));
            Assert.IsFalse(attribute.IsValid(null));
            Assert.IsTrue(attribute.IsValid("True"));
            Assert.IsTrue(attribute.IsValid("False"));
            Assert.IsTrue(attribute.IsValid(true));
            Assert.IsTrue(attribute.IsValid(false));
        }
    }
}