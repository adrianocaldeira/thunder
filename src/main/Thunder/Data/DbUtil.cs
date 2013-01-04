using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using Environment = NHibernate.Cfg.Environment;

namespace Thunder.Data
{
    /// <summary>
    /// Utility data base
    /// </summary>
    public class DbUtil
    {
        private const string MessageException =
            "The IDbCommand and IDbConnection implementation in the assembly {0} could not be found.";

        ///<summary>
        /// Initialize new instance off class <see cref="DbUtil"/>.
        ///</summary>
        public DbUtil()
        {
            Dialect = NHibernate.Dialect.Dialect.GetDialect(SessionManager.Configuration.Properties);
        }

        /// <summary>
        /// Get dialect
        /// </summary>
        public NHibernate.Dialect.Dialect Dialect { get; private set; }
        
        /// <summary>
        /// Create all objects in database
        /// </summary>
        public DbUtil CreateSchema()
        {
            new SchemaExport(SessionManager.Configuration).Create(false, true);
            return this;
        }

        /// <summary>
        /// Create database
        /// </summary>
        public DbUtil CreateDataBase(string name)
        {
            if (Dialect is MySQLDialect)
            {
                ExecuteCommand(string.Format("CREATE DATABASE IF NOT EXISTS `{0}`;", name));
            }

            return this;
        }


        public static void Drop()
        {
            var dialect = NHibernate.Dialect.Dialect.GetDialect(SessionManager.Configuration.Properties);
            var connection = Connection(dialect);

            if (dialect is MySQLDialect)
            {
                ExecuteCommand(string.Format("DROP DATABASE IF EXISTS `{0}`;", connection.Database), connection);
            }
        }

        public static void Create()
        {
            var dialect = NHibernate.Dialect.Dialect.GetDialect(SessionManager.Configuration.Properties);
            var connection = Connection(dialect);

            if (dialect is MySQLDialect)
            {
                connection.ConnectionString = connection.ConnectionString.Replace(string.Format("database={0};", connection.Database), "")
                    .Replace(string.Format("database={0}", connection.Database), "");
                ExecuteCommand(string.Format("CREATE DATABASE IF NOT EXISTS `{0}`;", connection.Database), connection);
            }
        }

        /// <summary>
        /// Drop all objects from database
        /// </summary>
        public DbUtil DropSchema()
        {
            new SchemaExport(SessionManager.Configuration).Drop(false, true);
            return this;
        }

        /// <summary>
        /// Generate script of all objects
        /// </summary>
        /// <param name="file">Path of file</param>
        /// <param name="delimiter">Delimiter</param>
        public void Script(string file, string delimiter = ";")
        {
            var schema = new SchemaExport(SessionManager.Configuration);
            schema.SetOutputFile(file);
            schema.SetDelimiter(delimiter);
            schema.Create(true, false);
        }

        /// <summary>
        /// Clear data of class type
        /// </summary>
        /// <param name="classType">Class Type</param>
        /// <returns><see cref="DbUtil"/></returns>
        public DbUtil Clear(Type @classType)
        {
            ExecuteClear(@classType);
            return this;
        }

        /// <summary>
        /// Bind
        /// </summary>
        /// <returns></returns>
        public DbUtil Bind()
        {
            SessionManager.Bind();
            return this;
        }

        /// <summary>
        /// Unbind
        /// </summary>
        /// <returns></returns>
        public DbUtil Unbind()
        {
            SessionManager.Unbind();
            return this;
        }

        private void ExecuteClear(Type @classType)
        {
            string table = GetTable(@classType);

            DisableConstraints(table);

            ExecuteCommand(string.Format("delete from {0}", table));

            EnableConstraints(table);

            ResetAutoIncrement(table, @classType);
        }

        private PersistentClass GetPersistentClass(Type @classType)
        {
            return SessionManager.Configuration.ClassMappings.FirstOrDefault(persistentClass => 
                @classType.FullName == persistentClass.EntityName);
        }

        private string GetTable(Type @classType)
        {
            PersistentClass persistentClass = GetPersistentClass(@classType);
            return persistentClass == null ? string.Empty : persistentClass.Table.Name;
        }

        private bool AllowResetAutoIncrement(string table, Type @classType)
        {
            PersistentClass persistentClass = GetPersistentClass(@classType);
            return !persistentClass.IsJoinedSubclass && persistentClass.IdentityTable.Name.Equals(table);
        }

        private void ResetAutoIncrement(string table, Type @classType)
        {
            if (Dialect is MsSql2000Dialect && AllowResetAutoIncrement(table, @classType))
            {
                ExecuteCommand(string.Format("DBCC CHECKIDENT('{0}', RESEED, 0) ;", table));
            }
            else if (Dialect is MySQLDialect && AllowResetAutoIncrement(table, @classType))
            {
                ExecuteCommand(string.Format("ALTER TABLE {0} AUTO_INCREMENT = 1;", table));
            }
            else if (Dialect is SQLiteDialect)
            {
                ExecuteCommand(string.Format("DELETE FROM sqlite_sequence WHERE name = '{0}';", table));
            }
        }

        private void DisableConstraints(string table)
        {
            if (Dialect is MsSql2000Dialect)
            {
                ExecuteCommand(string.Format("ALTER TABLE \"{0}\" NOCHECK CONSTRAINT ALL", table));
            }
            else if (Dialect is SQLiteDialect)
            {
                ExecuteCommand("PRAGMA foreign_keys = OFF;");
            }
            else if (Dialect is MySQLDialect)
            {
                ExecuteCommand("SET foreign_key_checks = 0;");
            }
            else if (Dialect is FirebirdDialect)
            {
                ExecuteCommand(string.Format("ALTER TABLE \"{0}\" DISABLE TRIGGER ALL", table));
            }
        }

        private void EnableConstraints(string table)
        {
            if (Dialect is MsSql2000Dialect)
            {
                ExecuteCommand(string.Format("ALTER TABLE \"{0}\" CHECK CONSTRAINT ALL", table));
            }
            else if (Dialect is SQLiteDialect)
            {
                ExecuteCommand("PRAGMA foreign_keys = ON;");
            }
            else if (Dialect is MySQLDialect)
            {
                ExecuteCommand("SET foreign_key_checks = 1;");
            }
            else if (Dialect is FirebirdDialect)
            {
                ExecuteCommand(string.Format("ALTER TABLE \"{0}\" ENABLE TRIGGER ALL", table));
            }
        }

        private void ExecuteCommand(string commandText)
        {
            using (IDbConnection connection = Connection(Dialect))
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        private static void ExecuteCommand(string commandText, IDbConnection dbConnection)
        {
            using (var connection = dbConnection)
            {
                connection.Open();

                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        private static IDbConnection Connection(NHibernate.Dialect.Dialect dialect)
        {
            IDbConnection connection = null;

            if (dialect is MsSql2000Dialect)
            {
                connection = new SqlConnection();
            }
            else if (dialect is SQLiteDialect)
            {
                connection = ObjectFactory<IDbConnection>("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            }
            else if (dialect is MySQLDialect)
            {
                connection = ObjectFactory<IDbConnection>("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            }
            else if (dialect is FirebirdDialect)
            {
                connection = ObjectFactory<IDbConnection>("FirebirdSql.Data.FirebirdClient",
                                                          "FirebirdSql.Data.FirebirdClient.FbConnection");
            }
            
            if (connection != null)
            {
                connection.ConnectionString = SessionManager.Configuration.GetProperty(Environment.ConnectionString);
            }

            return connection;
        }

        private static T ObjectFactory<T>(string assemblyName, string typeName)
        {
            try
            {
                Assembly assembly = Assembly.Load(assemblyName);
                Type type = assembly.GetType(typeName);

                return (T) Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(MessageException, assemblyName), ex);
            }
        }


    }
}