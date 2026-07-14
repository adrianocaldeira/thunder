using System;
using System.Security.Cryptography;

namespace Thunder.Security
{
    /// <summary>
    /// Provides password hashing and verification using PBKDF2-SHA256 with a random salt.
    /// Replaces the legacy <c>Hash</c>/<c>HashHelper</c> API (removed) for password storage.
    /// </summary>
    public static class PasswordHasher
    {
        #region Constants

        private const int SaltSize = 16;
        private const int SubkeySize = 32;
        private const int Iterations = 600000;
        private const int MinIterations = 100000;
        private const int MaxIterations = 2000000;

        #endregion

        #region Public methods

        /// <summary>
        /// Hashes <paramref name="password"/> with a random 16-byte salt using PBKDF2-SHA256
        /// (600,000 iterations). The result is a self-describing string in the format
        /// <c>"{iterations}.{Base64(salt)}.{Base64(subkey)}"</c>, ready to be persisted and later
        /// validated with <see cref="Verify"/>.
        /// </summary>
        /// <param name="password">Password to hash.</param>
        /// <returns>String representation of the password hash.</returns>
        public static string Hash(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            var salt = new byte[SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] subkey;

            using (var kdf = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                subkey = kdf.GetBytes(SubkeySize);
            }

            return Iterations + "." + Convert.ToBase64String(salt) + "." + Convert.ToBase64String(subkey);
        }

        /// <summary>
        /// Verifies whether <paramref name="password"/> matches the <paramref name="hash"/> previously
        /// produced by <see cref="Hash"/>. Re-derives the subkey with the same salt/iterations and
        /// compares it in constant time. The iteration count parsed from <paramref name="hash"/> is
        /// required to fall within [<c>100,000</c>, <c>2,000,000</c>]; values outside that band are
        /// treated as an invalid hash and rejected before any key derivation is attempted, preventing
        /// a corrupted or forged hash with an absurd iteration count from stalling the caller. Never
        /// throws for a wrong password or a malformed hash: it simply returns <c>false</c>.
        /// </summary>
        /// <param name="password">Password informed by the user.</param>
        /// <param name="hash">Hash previously produced by <see cref="Hash"/>.</param>
        /// <returns><c>true</c> when the password matches; otherwise <c>false</c>.</returns>
        public static bool Verify(string password, string hash)
        {
            if (password == null || string.IsNullOrEmpty(hash))
            {
                return false;
            }

            var parts = hash.Split('.');

            if (parts.Length != 3)
            {
                return false;
            }

            int iterations;

            if (!int.TryParse(parts[0], out iterations) || iterations < MinIterations || iterations > MaxIterations)
            {
                return false;
            }

            byte[] salt;
            byte[] expectedSubkey;

            try
            {
                salt = Convert.FromBase64String(parts[1]);
                expectedSubkey = Convert.FromBase64String(parts[2]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actualSubkey;

            try
            {
                using (var kdf = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    actualSubkey = kdf.GetBytes(expectedSubkey.Length);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return CryptoUtil.FixedTimeEquals(expectedSubkey, actualSubkey);
        }

        #endregion
    }
}
