using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Thunder.Security
{
    /// <summary>
    /// Provides authenticated symmetric encryption using AES-256-CBC combined with HMAC-SHA256
    /// in an Encrypt-then-MAC construction. Replaces the legacy <see cref="Cryptography"/> API,
    /// which uses a fixed IV and a weak key derivation.
    /// </summary>
    public static class AesEncryptor
    {
        #region Constants

        private const int SaltSize = 16;
        private const int IvSize = 16;
        private const int MacSize = 32;
        private const int KeySize = 32;
        private const int Iterations = 600000;
        private const byte Version = 0x01;

        #endregion

        #region Public methods

        /// <summary>
        /// Encrypts <paramref name="plaintext"/> with a key derived from <paramref name="password"/>
        /// using AES-256-CBC, appending an HMAC-SHA256 authentication tag over version, salt, IV and
        /// ciphertext (Encrypt-then-MAC).
        /// </summary>
        /// <param name="plaintext">Text to encrypt.</param>
        /// <param name="password">Password used to derive the encryption and MAC keys.</param>
        /// <returns>Base64 payload containing version, salt, IV, ciphertext and MAC.</returns>
        public static string Encrypt(string plaintext, string password)
        {
            if (plaintext == null)
            {
                throw new ArgumentNullException("plaintext");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var salt = new byte[SaltSize];
            var iv = new byte[IvSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
                rng.GetBytes(iv);
            }

            byte[] aesKey;
            byte[] macKey;

            using (var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                aesKey = kdf.GetBytes(KeySize);
                macKey = kdf.GetBytes(KeySize);
            }

            byte[] cipherBytes;

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = aesKey;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor())
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        var plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    cipherBytes = memoryStream.ToArray();
                }
            }

            var signedData = new byte[1 + SaltSize + IvSize + cipherBytes.Length];
            var offset = 0;

            signedData[offset] = Version;
            offset += 1;

            Buffer.BlockCopy(salt, 0, signedData, offset, SaltSize);
            offset += SaltSize;

            Buffer.BlockCopy(iv, 0, signedData, offset, IvSize);
            offset += IvSize;

            Buffer.BlockCopy(cipherBytes, 0, signedData, offset, cipherBytes.Length);

            byte[] mac;

            using (var hmac = new HMACSHA256(macKey))
            {
                mac = hmac.ComputeHash(signedData);
            }

            var result = new byte[signedData.Length + MacSize];
            Buffer.BlockCopy(signedData, 0, result, 0, signedData.Length);
            Buffer.BlockCopy(mac, 0, result, signedData.Length, MacSize);

            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// Decrypts a payload produced by <see cref="Encrypt"/>. The HMAC-SHA256 authentication tag
        /// is verified in constant time before any decryption is attempted, which prevents padding
        /// oracle attacks and ensures a wrong password never returns garbage: it throws instead.
        /// </summary>
        /// <param name="ciphertext">Base64 payload produced by <see cref="Encrypt"/>.</param>
        /// <param name="password">Password used to derive the encryption and MAC keys.</param>
        /// <returns>The original plaintext.</returns>
        /// <exception cref="CryptographicException">
        /// Thrown when the payload is malformed, or when HMAC verification fails (tampered
        /// ciphertext or wrong password).
        /// </exception>
        public static string Decrypt(string ciphertext, string password)
        {
            if (ciphertext == null)
            {
                throw new ArgumentNullException("ciphertext");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            byte[] data;

            try
            {
                data = Convert.FromBase64String(ciphertext);
            }
            catch (FormatException ex)
            {
                throw new CryptographicException("Payload em Base64 invalido.", ex);
            }

            var minLength = 1 + SaltSize + IvSize + MacSize;

            if (data.Length < minLength)
            {
                throw new CryptographicException("Payload invalido: tamanho insuficiente.");
            }

            if (data[0] != Version)
            {
                throw new CryptographicException("Versao de payload nao suportada.");
            }

            var salt = new byte[SaltSize];
            var iv = new byte[IvSize];
            var cipherLength = data.Length - minLength;
            var cipherBytes = new byte[cipherLength];
            var mac = new byte[MacSize];

            var offset = 1;

            Buffer.BlockCopy(data, offset, salt, 0, SaltSize);
            offset += SaltSize;

            Buffer.BlockCopy(data, offset, iv, 0, IvSize);
            offset += IvSize;

            Buffer.BlockCopy(data, offset, cipherBytes, 0, cipherLength);
            offset += cipherLength;

            Buffer.BlockCopy(data, offset, mac, 0, MacSize);

            byte[] aesKey;
            byte[] macKey;

            using (var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                aesKey = kdf.GetBytes(KeySize);
                macKey = kdf.GetBytes(KeySize);
            }

            var signedLength = data.Length - MacSize;
            var signedData = new byte[signedLength];
            Buffer.BlockCopy(data, 0, signedData, 0, signedLength);

            byte[] expectedMac;

            using (var hmac = new HMACSHA256(macKey))
            {
                expectedMac = hmac.ComputeHash(signedData);
            }

            if (!CryptoUtil.FixedTimeEquals(expectedMac, mac))
            {
                throw new CryptographicException("Falha na verificacao de integridade (HMAC).");
            }

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = aesKey;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var memoryStream = new MemoryStream(cipherBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}
