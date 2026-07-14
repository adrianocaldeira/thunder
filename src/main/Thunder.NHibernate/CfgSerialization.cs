using System;
using System.IO;
using NHibernate.Cfg;

namespace Thunder.NHibernate
{
    /// <summary>
    /// Serialization Hibernate Configuration
    /// </summary>
    /// <remarks>
    /// O cache binário de configuração baseado em serialização binária insegura foi desativado:
    /// <see cref="Create"/> e <see cref="Load()"/>/<see cref="Load(string)"/> agora lançam
    /// <see cref="NotSupportedException"/> por risco de desserialização insegura (CWE-502).
    /// </remarks>
    [Obsolete("O cache binário de configuração foi desativado por risco de desserialização insegura (CWE-502). Será removido na 2.0.")]
    public class CfgSerialization
    {
        private readonly string _directory;
        private readonly string _fileName;

        /// <summary>
        /// Initialize new instance of <see cref="CfgSerialization"/>.
        /// </summary>
        /// <param name="fileName"></param>
        public CfgSerialization(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name no information.", "fileName");

            _directory = AppDomain.CurrentDomain.BaseDirectory;
            _fileName = fileName;
        }

        /// <summary>
        /// Get path serialization file
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            return Path.Combine(_directory, _fileName);
        }

        /// <summary>
        /// Identifies whether the serialized file exists
        /// </summary>
        /// <returns></returns>
        bool Exist()
        {
            return File.Exists(Path.Combine(_directory, _fileName));
        }

        /// <summary>
        /// Create configuration file serialized
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="NotSupportedException">
        /// Sempre lançada: o cache binário de configuração foi desativado por segurança (CWE-502).
        /// </exception>
        public void Create(Configuration configuration)
        {
            throw new NotSupportedException("O cache binário de configuração foi desativado por segurança (CWE-502).");
        }

        /// <summary>
        /// Delete file serialized
        /// </summary>
        public void Delete()
        {
            if (Exist())
            {
                File.Delete(GetPath());
            }
        }

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <returns>Configuration</returns>
        /// <exception cref="NotSupportedException">
        /// Sempre lançada: o cache binário de configuração foi desativado por segurança (CWE-502).
        /// </exception>
        public Configuration Load()
        {
            return Load(null);
        }

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Configuration</returns>
        /// <exception cref="NotSupportedException">
        /// Sempre lançada: o cache binário de configuração foi desativado por segurança (CWE-502).
        /// </exception>
        public Configuration Load(string fileName)
        {
            throw new NotSupportedException("O cache binário de configuração foi desativado por segurança (CWE-502).");
        }
    }
}
