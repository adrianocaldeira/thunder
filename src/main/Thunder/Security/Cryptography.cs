using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Thunder.Security
{
    /// <summary>
    /// Cryptography security
    /// </summary>
    public class Cryptography
    {
        #region Constructors

        /// <summary>
        /// Initialize a new instance of the class <see cref="Cryptography"/>.
        /// </summary>
        public Cryptography()
        {
            _algorithm = new RijndaelManaged { Mode = CipherMode.CBC };
            _provider = CryptographyProvider.Rijndael;
            Encoding = Encoding.GetEncoding("ISO-8859-1");
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="Cryptography"/>.
        /// </summary>
        /// <param name="provider">Provider</param>
        public Cryptography(CryptographyProvider provider)
            : this()
        {
            switch (provider)
            {
                case CryptographyProvider.Rijndael:
                    _algorithm = new RijndaelManaged();
                    _provider = CryptographyProvider.Rijndael;
                    break;
                case CryptographyProvider.RC2:
                    _algorithm = new RC2CryptoServiceProvider();
                    _provider = CryptographyProvider.RC2;
                    break;
                case CryptographyProvider.DES:
                    _algorithm = new DESCryptoServiceProvider();
                    _provider = CryptographyProvider.DES;
                    break;
                case CryptographyProvider.TripleDES:
                    _algorithm = new TripleDESCryptoServiceProvider();
                    _provider = CryptographyProvider.TripleDES;
                    break;
            }
            _algorithm.Mode = CipherMode.CBC;
        }

        #endregion

        #region Fields

        private readonly CryptographyProvider _provider;
        private readonly SymmetricAlgorithm _algorithm;

        #endregion

        #region Properties

        /// <summary>
        /// Get and set key for criptography
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Get and set enconding
        /// </summary>
        public Encoding Encoding { get; set; }

        #endregion

        #region Private methods

        /// <summary>
        /// Set IV
        /// </summary>
        private void SetIv()
        {
            switch (_provider)
            {
                case CryptographyProvider.Rijndael:
                    _algorithm.IV = new byte[]
                                        {
                                            0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9, 0x5, 0x46, 0x9c, 0xea, 0xa8,
                                            0x4b, 0x73, 0xcc
                                        };
                    break;
                default:
                    _algorithm.IV = new byte[] { 0xf, 0x6f, 0x13, 0x2e, 0x35, 0xc2, 0xcd, 0xf9 };
                    break;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get salt key 
        /// </summary>
        /// <returns></returns>
        public virtual byte[] GetKey()
        {
            if (_algorithm.LegalKeySizes.Length > 0)
            {
                var keySize = Key.Length * 8;
                var minSize = _algorithm.LegalKeySizes[0].MinSize;
                var maxSize = _algorithm.LegalKeySizes[0].MaxSize;
                var skipSize = _algorithm.LegalKeySizes[0].SkipSize;

                if (keySize > maxSize)
                {
                    Key = Key.Substring(0, maxSize / 8);
                }
                else if (keySize < maxSize)
                {
                    var validSize = (keySize <= minSize) ? minSize : (keySize - keySize % skipSize) + skipSize;
                    if (keySize < validSize)
                    {
                        Key = Key.PadRight(validSize / 8, '*');
                    }
                }
            }

            return new Rfc2898DeriveBytes(Key, Encoding.GetBytes(Key)).GetBytes(Key.Length);
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        public virtual string Encrypt(string text)
        {
            var textBytes = Encoding.GetBytes(text);

            _algorithm.Key = GetKey();

            SetIv();

            var cryptoTransform = _algorithm.CreateEncryptor();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);

            cryptoStream.Write(textBytes, 0, textBytes.Length);
            cryptoStream.FlushFinalBlock();

            var bytes = memoryStream.ToArray();

            return Convert.ToBase64String(bytes, 0, bytes.GetLength(0));
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        public virtual string Decrypt(string text)
        {
            var textBytes = Convert.FromBase64String(text);

            _algorithm.Key = GetKey();

            SetIv();

            var cryptoTransform = _algorithm.CreateDecryptor();

            try
            {
                var memoryStream = new MemoryStream(textBytes, 0, textBytes.Length);
                var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);

                return new StreamReader(cryptoStream).ReadToEnd();
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
