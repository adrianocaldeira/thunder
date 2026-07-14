namespace Thunder.Security
{
    /// <summary>
    /// Internal cryptographic utilities shared by <see cref="AesEncryptor"/> and <see cref="PasswordHasher"/>.
    /// </summary>
    internal static class CryptoUtil
    {
        /// <summary>
        /// Compares two byte arrays in constant time (does not short-circuit on the first difference),
        /// avoiding timing side-channel attacks. The .NET Framework 4.8 BCL has no
        /// <c>CryptographicOperations.FixedTimeEquals</c>, so it is implemented manually here.
        /// </summary>
        /// <param name="a">First array.</param>
        /// <param name="b">Second array.</param>
        /// <returns><c>true</c> when both arrays have the same length and the same content.</returns>
        internal static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var result = 0;

            for (var i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}
