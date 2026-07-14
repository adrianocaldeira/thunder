using NUnit.Framework;

namespace Thunder.Security
{
    [TestFixture]
    class LegacyCompatTest
    {
        /// <summary>
        /// A API legada (marcada com <c>[Obsolete]</c>) precisa continuar funcionando em runtime:
        /// o consumidor real ainda decifra senhas ja gravadas com <see cref="Cryptography"/>.
        /// </summary>
        [Test]
#pragma warning disable 618
        public void LegacyCryptographyStillRoundTrips()
        {
            var cryptography = new Cryptography { Key = "k" };

            var encrypted = cryptography.Encrypt("x");

            Assert.AreEqual("x", new Cryptography { Key = "k" }.Decrypt(encrypted));
        }
#pragma warning restore 618
    }
}
