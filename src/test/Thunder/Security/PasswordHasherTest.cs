using NUnit.Framework;

namespace Thunder.Security
{
    [TestFixture]
    class PasswordHasherTest
    {
        [Test]
        public void VerifyReturnsTrueForCorrectPassword()
        {
            const string password = "MinhaSenh@123";

            var hash = PasswordHasher.Hash(password);

            Assert.IsTrue(PasswordHasher.Verify(password, hash));
        }

        [Test]
        public void VerifyReturnsFalseForWrongPassword()
        {
            const string password = "senha-correta";

            var hash = PasswordHasher.Hash(password);

            Assert.IsFalse(PasswordHasher.Verify("senha-errada", hash));
        }

        [Test]
        public void HashIsSaltedAndNonDeterministic()
        {
            const string password = "mesma-senha";

            var first = PasswordHasher.Hash(password);
            var second = PasswordHasher.Hash(password);

            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void VerifyReturnsFalseForMalformedHash()
        {
            Assert.IsFalse(PasswordHasher.Verify("qualquer", "abc"));
        }

        [Test]
        public void HashHasThreePartsSeparatedByDot()
        {
            var hash = PasswordHasher.Hash("senha");

            var parts = hash.Split('.');

            Assert.AreEqual(3, parts.Length);
        }
    }
}
