using System;
using System.Text;

namespace Thunder.Security
{
    /// <summary>
    /// Security extensions
    /// </summary>
    public static class HashHelper
    {
        /// <summary>
        /// Get hash text with encoding ISO-8859-1 and SHA1 provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Hash</returns>
        [Obsolete("Use Thunder.Security.PasswordHasher para senhas, ou um algoritmo SHA-256+ explícito. Será removido na 2.0.")]
        public static string Hash(this string text)
        {
            return text.Hash(Encoding.GetEncoding("ISO-8859-1"));
        }

        /// <summary>
        /// Get hash text with encoding and SHA1 provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Hash</returns>
        [Obsolete("Use Thunder.Security.PasswordHasher para senhas, ou um algoritmo SHA-256+ explícito. Será removido na 2.0.")]
        public static string Hash(this string text, Encoding encoding)
        {
            return text.Hash(encoding, HashProvider.SHA1);
        }

        /// <summary>
        /// Get hash text with encoding and provider
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="encoding">Encoding</param>
        /// <param name="provider">Provider</param>
        /// <returns>Hash</returns>
        [Obsolete("Use Thunder.Security.PasswordHasher para senhas, ou um algoritmo SHA-256+ explícito. Será removido na 2.0.")]
        public static string Hash(this string text, Encoding encoding, HashProvider provider)
        {
#pragma warning disable 618
            var hash = new Hash(provider) {Encoding = encoding};

            return hash.Get(text);
#pragma warning restore 618
        }
    }
}