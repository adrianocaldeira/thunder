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
            var connectionBuilder = new ConnectionBuilder(Dialect, Connection);

            if (Dialect is MySQLDialect)
            {
                Connection.ConnectionString = connectionBuilder
                    .With("Server", connectionBuilder.GetValueOfPart("Server"))
                    .With("User Id", connectionBuilder.GetValueOfPart("User Id"))
                    .With("Password", connectionBuilder.GetValueOfPart("Password"))
                    .Builder()
                    .ConnectionString;

                ExecuteCommand(string.Format("CREATE DATABASE IF NOT EXISTS {0};", Connection.Database), Connection);
            }
            else if (Dialect is MsSql2000Dialect)
            {
                Connection.ConnectionString = connectionBuilder
                    .With("Server", connectionBuilder.GetValueOfPart("Server"))
                    .With("User Id", connectionBuilder.GetValueOfPart("User Id"))
                    .With("Password", connectionBuilder.GetValueOfPart("Password"))
                    .Builder()
                    .ConnectionString;

                ExecuteCommand(string.Format("IF NOT(EXISTS(SELECT * FROM sys.sysdatabases where name='{0}')) BEGIN CREATE DATABASE {0}; END", Connection.Database), Connection);
            }

            Connection = ConnectionFactory.Factory(Configuration, Dialect);

            new SchemaExport(SessionManager.Configuration).Create(false, true);

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
            var connectionBuilder = new ConnectionBuilder(Dialect, Connection);

            if (Dialect is MySQLDialect)
            {
                Connection.ConnectionString = connectionBuilder
                    .With("Server", connectionBuilder.GetValueOfPart("Server"))
                    .With("User Id", connectionBuilder.GetValueOfPart("User Id"))
                    .With("Password", connectionBuilder.GetValueOfPart("Password"))
                    .Builder()
                    .ConnectionString;

                ExecuteCommand(string.Format("DROP DATABASE IF EXISTS {0};", Connection.Database), Connection);
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
                connection.Open();

                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
            }
        }
       
    }
}