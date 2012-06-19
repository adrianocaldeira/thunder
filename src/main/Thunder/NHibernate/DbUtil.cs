using System;
using System.Data;
using System.Linq;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;

namespace Thunder.NHibernate
{
    /// <summary>
    /// Utility data base
    /// </summary>
    public class DbUtil
    {
        private const string MessageException = "The IDbCommand and IDbConnection implementation in the assembly {0} could not be found.";

        ///<summary>
        /// Initialize new instance off class <see cref="DbUtil"/>.
        ///</summary>
        ///<param name="configuration">Nhibernate configuration</param>
        public DbUtil(Configuration configuration)
        {
            Configuration = configuration;
            Dialect = global::NHibernate.Dialect.Dialect.GetDialect(Configuration.Properties);

        }

        /// <summary>
        /// Get NHibernate configuration
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Get dialect
        /// </summary>
        public global::NHibernate.Dialect.Dialect Dialect { get; private set; }


        /// <summary>
        /// Create data base schema
        /// </summary>
        public void Create()
        {
            new SchemaExport(Configuration).Drop(false, true);
            new SchemaExport(Configuration).Create(false, true);
        }

        /// <summary>
        /// Exporta schema data base
        /// </summary>
        /// <param name="file">Path of file</param>
        public void ExportSchema(string file)
        {
            var schemaExport = new SchemaExport(Configuration);
            schemaExport.SetOutputFile(file);
            schemaExport.Create(true, true);
        }

        /// <summary>
        /// Close NHibernate session
        /// </summary>
        /// <param name="session">Session</param>
        /// <returns><see cref="DbUtil"/></returns>
        public DbUtil Close(ISession session)
        {
            session.Close();
            session.Dispose();

            return this;
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

        private void ExecuteClear(Type @classType)
        {
            var table = GetTable(@classType);
            
            DisableConstraints(table);
            
            ExecuteCommand(string.Format("delete from {0}", table));
            
            EnableConstraints(table);

            ResetAutoIncrement(table, @classType);
        }

        private PersistentClass GetPersistentClass(Type @classType)
        {
            return Configuration.ClassMappings.Where(persistentClass => 
                @classType.FullName == persistentClass.EntityName).FirstOrDefault();
        }

        private string GetTable(Type @classType)
        {
            var persistentClass = GetPersistentClass(@classType);
            return persistentClass == null ? string.Empty : persistentClass.Table.Name;
        }

        private bool AllowResetAutoIncrement(string table, Type @classType)
        {
            var persistentClass = GetPersistentClass(@classType);
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
            else if(Dialect is SQLiteDialect)
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
            using (var connection = Connection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }            
        }

        private IDbConnection Connection()
        {
            IDbConnection connection = null;

            if (Dialect is MsSql2000Dialect)
            {
                connection = new System.Data.SqlClient.SqlConnection();
            }
            else if(Dialect is SQLiteDialect)
            {
                connection = ObjectFactory<IDbConnection>("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            }
            else if (Dialect is MySQLDialect)
            {
                connection = ObjectFactory<IDbConnection>("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            }
            else if (Dialect is FirebirdDialect)
            {
                connection = ObjectFactory<IDbConnection>("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection");
            }

            if (connection != null)
            {
                connection.ConnectionString = Configuration.GetProperty(
                        global::NHibernate.Cfg.Environment.ConnectionString);                
            }

            return connection;
        }

        private static T ObjectFactory<T>(string assemblyName, string typeName)
        {
            try
            {
                var assembly = Assembly.Load(assemblyName);
                var type = assembly.GetType(typeName);

                return (T)Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(MessageException, assemblyName), ex);
            }
        }

        
    }
}