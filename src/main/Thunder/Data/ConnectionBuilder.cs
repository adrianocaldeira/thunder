using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NHibernate.Dialect;

namespace Thunder.Data
{
    /// <summary>
    /// Connection builder
    /// </summary>
    public class ConnectionBuilder
    {
        private StringBuilder _builder = new StringBuilder();

        /// <summary>
        /// Initialize new instance of <see cref="ConnectionBuilder"/>
        /// </summary>
        /// <param name="dialect"><see cref="NHibernate.Dialect.Dialect"/></param>
        /// <param name="connection"><see cref="IDbConnection"/></param>
        public ConnectionBuilder(NHibernate.Dialect.Dialect dialect, IDbConnection connection)
        {
            ConnectionString = connection.ConnectionString;
            Dialect = dialect;
            Parts = GetParts();
        }

        /// <summary>
        /// Get connection string
        /// </summary>
        public string ConnectionString { get; private set; }
        /// <summary>
        /// Get dialect
        /// </summary>
        public NHibernate.Dialect.Dialect Dialect { get; private set; }
        /// <summary>
        /// Get parts
        /// </summary>
        public Dictionary<string, string> Parts { get; private set; }

        /// <summary>
        /// Clear builder connection
        /// </summary>
        /// <returns></returns>
        public ConnectionBuilder Clear()
        {
            _builder = new StringBuilder();
            return this;
        }

        /// <summary>
        /// Increment connection
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns><see cref="ConnectionBuilder"/></returns>
        public ConnectionBuilder With(string key, string value)
        {
            _builder.Append(string.Format("{0}={1}{2}", key.ToLower(), value, Separator));

            return this;
        }

        /// <summary>
        /// Get valur of part connection
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public string GetValueOfPart(string key)
        {
            if(Parts.ContainsKey(key.ToLower()))
            {
                return Parts[key.ToLower()];
            }

            return string.Empty;
        }

        /// <summary>
        /// Builder new connection
        /// </summary>
        /// <returns><see cref="ConnectionBuilder"/></returns>
        public ConnectionBuilder Builder()
        {
            ConnectionString = _builder.ToString();

            return this;
        }

        /// <summary>
        /// Get separator
        /// </summary>
        private char Separator
        {
            get
            {
                if (Dialect is MySQLDialect || Dialect is PostgreSQLDialect)
                {
                    return ';';
                }
              
                return ' ';
            }
        }

        /// <summary>
        /// Get parts
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetParts()
        {
            return ConnectionString.Split(Separator).Select(item => item.Split('=')).ToDictionary(parts => parts[0].ToLower(),
                                                                                                  parts => parts[1]);
        }
    }
}
