using System;
using System.Text;

namespace Thunder.Security
{
    /// <summary>
    /// Security extensions
    /// </summary>
    public static class CryptographyHelper
    {
        /// <summary>
        /// Encripty text with encoding ISO-8859-1 and Rijndael provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="key">Key</param>
        /// <returns>Text</returns>
        [Obsolete("Use Thunder.Security.AesEncryptor (AES-256 + HMAC-SHA256). A criptografia legada é fraca (IV fixo, KDF fraco) e será removida na 2.0.")]
        public static string Encrypt(this string text, string key)
        {
            return text.Encrypt(key, Encoding.GetEncoding("ISO-8859-1"), CryptographyProvider.Rijndael);
        }

        /// <summary>
        /// Encripty text with encoding and Rijndael provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="key">Key</param>
        /// <param name="encoding">Encoding</param>
        /// <returns></returns>
        [Obsolete("Use Thunder.Security.AesEncryptor (AES-256 + HMAC-SHA256). A criptografia legada é fraca (IV fixo, KDF fraco) e será removida na 2.0.")]
        public static string Encrypt(this string text, string key, Encoding encoding)
        {
            return text.Encrypt(key, encoding, CryptographyProvider.Rijndael);
        }

        /// <summary>
        /// Encripty text with encoding and provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="key">Key</param>
        /// <param name="encoding">Encoding</param>
        /// <param name="provider">Provider</param>
        /// <returns>Text</returns>
        [Obsolete("Use Thunder.Security.AesEncryptor (AES-256 + HMAC-SHA256). A criptografia legada é fraca (IV fixo, KDF fraco) e será removida na 2.0.")]
        public static string Encrypt(this string text, string key, Encoding encoding, CryptographyProvider provider)
        {
#pragma warning disable 618
            var cryptography = new Cryptography(provider) {Key = key, Encoding = encoding};

            return cryptography.Encrypt(text);
#pragma warning restore 618
        }

        /// <summary>
        /// Decrypt text with encoding ISO-8859-1 and Rijndael provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="key">Key</param>
        /// <returns>Text</returns>
        [Obsolete("Use Thunder.Security.AesEncryptor (AES-256 + HMAC-SHA256). A criptografia legada é fraca (IV fixo, KDF fraco) e será removida na 2.0.")]
        public static string Decrypt(this string text, string key)
        {
            return text.Decrypt(key, Encoding.GetEncoding("ISO-8859-1"));
        }

        /// <summary>
        /// Decrypt text with encoding and Rijndael provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="key">Key</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Text</returns>
        [Obsolete("Use Thunder.Security.AesEncryptor (AES-256 + HMAC-SHA256). A criptografia legada é fraca (IV fixo, KDF fraco) e será removida na 2.0.")]
        public static string Decrypt(this string text, string key, Encoding encoding)
        {
            return text.Decrypt(key, encoding, CryptographyProvider.Rijndael);
        }

        /// <summary>
        /// Decrypt text with encoding and provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="key">Key</param>
        /// <param name="encoding">Encoding</param>
        /// <param name="provider">Provider</param>
        /// <returns>Text</returns>
        [Obsolete("Use Thunder.Security.AesEncryptor (AES-256 + HMAC-SHA256). A criptografia legada é fraca (IV fixo, KDF fraco) e será removida na 2.0.")]
        public static string Decrypt(this string text, string key, Encoding encoding, CryptographyProvider provider)
        {
#pragma warning disable 618
            var cryptography = new Cryptography(provider) {Key = key, Encoding = encoding};

            return cryptography.Decrypt(text);
#pragma warning restore 618
        }
    }
}