using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Thunder.Security
{
    [TestFixture]
    class AesEncryptorTest
    {
        [Test]
        public void RoundTrip()
        {
            const string plaintext = "informacao confidencial";
            const string password = "senha-forte";

            var ciphertext = AesEncryptor.Encrypt(plaintext, password);

            Assert.AreEqual(plaintext, AesEncryptor.Decrypt(ciphertext, password));
        }

        [Test]
        public void EncryptIsNonDeterministic()
        {
            const string plaintext = "mesmo texto";
            const string password = "mesma-senha";

            var first = AesEncryptor.Encrypt(plaintext, password);
            var second = AesEncryptor.Encrypt(plaintext, password);

            Assert.AreNotEqual(first, second);
        }

        [Test]
        public void TamperedCiphertextThrowsCryptographicException()
        {
            const string plaintext = "texto original com conteudo suficiente para ter mais de um bloco AES";
            const string password = "senha";

            var ciphertext = AesEncryptor.Encrypt(plaintext, password);
            var bytes = Convert.FromBase64String(ciphertext);

            var middle = bytes.Length / 2;
            bytes[middle] ^= 0xFF;

            var tampered = Convert.ToBase64String(bytes);

            Assert.Throws<CryptographicException>(() => AesEncryptor.Decrypt(tampered, password));
        }

        [Test]
        public void WrongPasswordThrowsCryptographicException()
        {
            const string plaintext = "dado sensivel";

            var ciphertext = AesEncryptor.Encrypt(plaintext, "senha-correta");

            Assert.Throws<CryptographicException>(() => AesEncryptor.Decrypt(ciphertext, "senha-errada"));
        }

        [Test]
        public void EmptyTextRoundTrip()
        {
            var ciphertext = AesEncryptor.Encrypt(string.Empty, "senha");

            Assert.AreEqual(string.Empty, AesEncryptor.Decrypt(ciphertext, "senha"));
        }

        [Test]
        public void UnicodeTextRoundTrip()
        {
            const string plaintext = "Acentuacao: ção, é, ü, 日本語";
            const string password = "senha-unicode";

            var ciphertext = AesEncryptor.Encrypt(plaintext, password);

            Assert.AreEqual(plaintext, AesEncryptor.Decrypt(ciphertext, password));
        }
    }
}
