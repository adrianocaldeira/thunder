using NUnit.Framework;

namespace Thunder.ComponentModel.DataAnnotations
{
    [TestFixture]
    public class EmailAttributeTest
    {
        [Test]
        public void IsValid()
        {
            var attribute = new EmailAttribute();

            Assert.IsTrue(attribute.IsValid(null));            // null é responsabilidade do [Required]
            Assert.IsTrue(attribute.IsValid(""));               // string vazia também é responsabilidade do [Required]
            Assert.IsTrue(attribute.IsValid("a@b.com"));
            Assert.IsTrue(attribute.IsValid("a.b-c_d@dominio.com.br"));
            Assert.IsFalse(attribute.IsValid("a@"));           // domínio vazio
            Assert.IsFalse(attribute.IsValid("@b.com"));       // parte local vazia
            Assert.IsFalse(attribute.IsValid("a@b"));          // sem TLD
            Assert.IsFalse(attribute.IsValid("a b@c.com"));
        }
    }
}
