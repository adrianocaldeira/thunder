using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Cfg;

namespace Thunder.Data
{
    /// <summary>
    /// Serialization Hibernate Configuration
    /// </summary>
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
        /// Configuration file serialized is valid
        /// </summary>
        /// <returns></returns>
        bool IsValid()
        {
            if (Exist())
            {
                var assembly = Assembly.GetCallingAssembly();
                var fileInfo = new FileInfo(GetPath());
                var assemblyInfo = new FileInfo(assembly.Location);

                return fileInfo.LastWriteTime >= assemblyInfo.LastWriteTime;
            }

            return false;
        }

        /// <summary>
        /// Configuration file serialized is new
        /// </summary>
        /// <returns></returns>
        bool IsNew()
        {
            return !Exist() || !IsValid();
        }

        /// <summary>
        /// Create configuration file serialized
        /// </summary>
        /// <param name="configuration"></param>
        public void Create(Configuration configuration)
        {
            using (var file = File.Open(GetPath(), FileMode.Create))
            {
                var bf = new BinaryFormatter();
                bf.Serialize(file, configuration);
            }
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
        public Configuration Load()
        {
            return Load(null);
        }

        /// <summary>
        /// Load configuration
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Configuration</returns>
        public Configuration Load(string fileName)
        {
            if (IsNew())
            {
                Create(string.IsNullOrEmpty(fileName)
                           ? new Configuration().Configure()
                           : new Configuration().Configure(fileName));
            }

            using (var file = File.Open(GetPath(), FileMode.Open))
            {
                var bf = new BinaryFormatter();
                var config = bf.Deserialize(file) as Configuration;

                return config;
            }
        }
    }
}
