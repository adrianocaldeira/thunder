using System;
using System.Configuration;
using NHibernate;
using NHibernate.Context;
using NHibernate.Event;
using Thunder.Data.Pattern;
using Configuration = NHibernate.Cfg.Configuration;

namespace Thunder.Data
{
    /// <summary>
    /// NHibernate Session Manager
    /// </summary>
    public sealed class SessionManager
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        private static bool? _serializeConfiguration;

        /// <summary>
        /// Get current instance of <see cref="ISessionFactory"/>.
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Configuration.BuildSessionFactory();

                    if (_sessionFactory == null)
                        throw new InvalidOperationException("Call to configuration.BuildSessionFactory() returned null.");
                }

                return _sessionFactory;
            }
        }

        /// <summary>
        /// Get if serialize configuration
        /// </summary>
        public static bool SerializeConfiguration
        {
            get
            {
                if (_serializeConfiguration != null) return _serializeConfiguration.Value;

                try
                {
                    _serializeConfiguration = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["Thunder.Data.SessionManager.SerializeConfiguration"]) && 
                        Convert.ToBoolean(ConfigurationManager.AppSettings["Thunder.Data.SessionManager.SerializeConfiguration"]);
                }
                catch (Exception)
                {
                    _serializeConfiguration = false;
                }

                return _serializeConfiguration.Value;    
            }
        }

        /// <summary>
        /// Get hibernate configuration
        /// </summary>
        /// <returns><see cref="Configuration"/></returns>
        public static Configuration Configuration
        {
            get
            {
                if (_configuration != null) return _configuration;

                if (SerializeConfiguration)
                {
                    _configuration = new CfgSerialization("cfg.thunder").Load();
                }
                else
                {
                    _configuration = new Configuration().Configure();

                    _configuration.AppendListeners(ListenerType.PreUpdate, new IPreUpdateEventListener[] { new CreatedAndUpdatedPropertyEventListener() });
                    _configuration.AppendListeners(ListenerType.PreInsert, new IPreInsertEventListener[] { new CreatedAndUpdatedPropertyEventListener() });
                }

                if (_configuration == null)
                    throw new InvalidOperationException("NHibernate configuration is null.");

                return _configuration;
            }
        }

        /// <summary>
        /// Get current session
        /// </summary>
        public static ISession CurrentSession
        {
            get { return SessionFactory.GetCurrentSession(); }
        }

        /// <summary>
        /// Bind nhibernate session
        /// </summary>
        public static void Bind()
        {
            CurrentSessionContext.Bind(SessionFactory.OpenSession());
        }

        /// <summary>
        /// Unbind nhibernate session
        /// </summary>
        public static void Unbind()
        {
            var session = CurrentSessionContext.Unbind(SessionFactory);

            if (session == null || !session.IsOpen) return;

            try
            {
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Rollback();
                }
                else
                {
                    session.Flush();
                }
            }
            finally
            {
                session.Close();
                session.Dispose();
            }
        }
    }
}