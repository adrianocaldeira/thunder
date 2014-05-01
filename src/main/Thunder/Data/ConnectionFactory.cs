using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using Environment = NHibernate.Cfg.Environment;

namespace Thunder.Data
{
    /// <summary>
    /// Connection factory
    /// </summary>
    public static class ConnectionFactory
    {
        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="configuration"><see cref="Configuration"/></param>
        /// <param name="dialect"><see cref="NHibernate.Dialect.Dialect"/></param>
        /// <returns><see cref="IDbConnection"/></returns>
        public static IDbConnection Factory(Configuration configuration, NHibernate.Dialect.Dialect dialect)
        {
            IDbConnection connection = null;

            if (dialect is MsSql2000Dialect)
            {
                connection = new SqlConnection();
            }
            else if (dialect is SQLiteDialect)
            {
                connection = CreateObject<IDbConnection>("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            }
            else if (dialect is MySQLDialect)
            {
                connection = CreateObject<IDbConnection>("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            }
            else if (dialect is FirebirdDialect)
            {
                connection = CreateObject<IDbConnection>("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection");
            }
            else if (dialect is PostgreSQLDialect)
            {
                connection = CreateObject<IDbConnection>("Npgsql", "Npgsql.NpgsqlConnection");
            }

            if (connection != null)
            {
                connection.ConnectionString = configuration.GetProperty(Environment.ConnectionString);
            }

            return connection;
        }

        /// <summary>
        /// Create object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="assemblyName">Assembly name</param>
        /// <param name="typeName">Type name</param>
        /// <returns>Object</returns>
        private static T CreateObject<T>(string assemblyName, string typeName)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                var type = assembly.GetType(typeName);

                return (T)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format(
                        "The IDbCommand and IDbConnection implementation in the assembly {0} could not be found.",
                        assemblyName), ex);
            }
        }
    }
}
