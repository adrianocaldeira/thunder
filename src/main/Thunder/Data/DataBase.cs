using System.Data;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;

namespace Thunder.Data
{
    /// <summary>
    /// Data base utility
    /// </summary>
    public class DataBase
    {
        /// <summary>
        /// Initialize new instance of <see cref="DataBase"/>
        /// </summary>
        /// <param name="configuration"><see cref="Configuration"/></param>
        public DataBase(Configuration configuration)
        {
            Configuration = configuration;
            Dialect = NHibernate.Dialect.Dialect.GetDialect(Configuration.Properties);
            Connection = ConnectionFactory.Factory(Configuration, Dialect);
        }

        /// <summary>
        /// Get configuration
        /// </summary>
        public Configuration Configuration { get; private set; }

        /// <summary>
        /// Get dialect
        /// </summary>
        public NHibernate.Dialect.Dialect Dialect { get; private set; }

        /// <summary>
        /// Get connection
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// Creata data base
        /// </summary>
        /// <returns><see cref="DataBase"/></returns>
        public DataBase Create()
        {
            var database = Connection.Database;

            Connection.ConnectionString = GetConnectionString();

            if (Dialect is MySQLDialect)
            {
                ExecuteCommand(string.Format("CREATE DATABASE IF NOT EXISTS {0};", database), Connection);
            }
            else if (Dialect is MsSql2000Dialect)
            {
                ExecuteCommand(string.Format("IF NOT(EXISTS(SELECT * FROM sys.sysdatabases where name='{0}')) BEGIN CREATE DATABASE {0}; END", database), Connection);
            }
            else if (Dialect is PostgreSQLDialect)
            {
                Connection.ChangeDatabase("postgres");

                ExecuteCommand(string.Format("CREATE DATABASE {0};", database), Connection);
            }

            Connection = ConnectionFactory.Factory(Configuration, Dialect);

            var schemaExport = new SchemaExport(SessionManager.Configuration);

            schemaExport.Create(false, true);

            return this;
        }

        /// <summary>
        /// Update data base
        /// </summary>
        /// <returns></returns>
        public DataBase Update()
        {
            new SchemaUpdate(SessionManager.Configuration).Execute(false, true);

            return this;
        }

        /// <summary>
        /// Drop data base
        /// </summary>
        /// <returns><see cref="DataBase"/></returns>
        public DataBase Drop()
        {
            var database = Connection.Database;

            Connection.ConnectionString = GetConnectionString();
            
            if (Dialect is MySQLDialect)
            {
                ExecuteCommand(string.Format("DROP DATABASE IF EXISTS {0};", database), Connection);
            }
            else if (Dialect is MsSql2000Dialect)
            {
                ExecuteCommand(string.Format("IF (EXISTS(SELECT * FROM sys.sysdatabases where name='{0}')) BEGIN DROP DATABASE {0}; END", database), Connection);
            }
            else if (Dialect is PostgreSQLDialect)
            {
                Connection.ChangeDatabase("postgres");

                ExecuteCommand(string.Format("DROP DATABASE IF EXISTS {0};", database), Connection);
            }

            Connection = ConnectionFactory.Factory(Configuration, Dialect);

            return this;
        }

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="dbConnection"></param>
        public void ExecuteCommand(string commandText, IDbConnection dbConnection)
        {
            using (var connection = dbConnection)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();                    
                }

                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }

        private string GetConnectionString()
        {
            var connectionBuilder = new ConnectionBuilder(Dialect, Connection);

            if (Dialect is MySQLDialect)
            {
                return connectionBuilder
                    .With("Server", connectionBuilder.GetValueOfPart("Server"))
                    .With("User Id", connectionBuilder.GetValueOfPart("User Id"))
                    .With("Password", connectionBuilder.GetValueOfPart("Password"))
                    .Builder()
                    .ConnectionString;
            }

            if (Dialect is MsSql2000Dialect)
            {
                return connectionBuilder
                    .With("Server", connectionBuilder.GetValueOfPart("Server"))
                    .With("User Id", connectionBuilder.GetValueOfPart("User Id"))
                    .With("Password", connectionBuilder.GetValueOfPart("Password"))
                    .Builder()
                    .ConnectionString;
            }
            
            if (Dialect is PostgreSQLDialect)
            {
                return connectionBuilder
                    .With("Host", connectionBuilder.GetValueOfPart("Host"))
                    .With("User Id", connectionBuilder.GetValueOfPart("User Id"))
                    .With("Password", connectionBuilder.GetValueOfPart("Password"))
                    .With("Port", connectionBuilder.GetValueOfPart("Port"))
                    .With("DataBase", connectionBuilder.GetValueOfPart("DataBase"))
                    .Builder()
                    .ConnectionString;
            }

            return string.Empty;
        }
    }
}