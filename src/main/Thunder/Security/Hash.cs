using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Thunder.Security
{
    /// <summary>
    /// Hash security
    /// </summary>
    public class Hash
    {
        #region Constructors
        /// <summary>
        /// Initialize a new instance of the class <see cref="Hash"/>.
        /// </summary>
        public Hash()
        {
            _algorithm = new SHA1Managed();
            Encoding = Encoding.GetEncoding("iso-8859-1");
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="Hash"/>.
        /// </summary>
        /// <param name="provider">Provider</param>
        public Hash(HashProvider provider)
            : this()
        {
            switch (provider)
            {
                case HashProvider.MD5:
                    _algorithm = new MD5CryptoServiceProvider();
                    break;
                case HashProvider.SHA1:
                    _algorithm = new SHA1Managed();
                    break;
                case HashProvider.SHA256:
                    _algorithm = new SHA256Managed();
                    break;
                case HashProvider.SHA384:
                    _algorithm = new SHA384Managed();
                    break;
                case HashProvider.SHA512:
                    _algorithm = new SHA512Managed();
                    break;
            }
        }
        #endregion

        #region Fields
        private readonly HashAlgorithm _algorithm;
        #endregion

        #region Properties
        /// <summary>
        /// Get and set enconding
        /// </summary>
        public Encoding Encoding { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Get hash
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Hash</returns>
        public string Get(string text)
        {
            var bytes = _algorithm.ComputeHash(Encoding.GetBytes(text));

            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
        /// <summary>
        /// Get hash
        /// </summary>
        /// <param name="fileStream">File stream</param>
        /// <returns>Hash</returns>
        public string Get(FileStream fileStream)
        {
            var bytes = _algorithm.ComputeHash(fileStream);
            fileStream.Close();

            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }
        #endregion
    }
}
