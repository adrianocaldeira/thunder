using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace Thunder.Data
{
    /// <summary>
    /// NHibernate Session Manager
    /// </summary>
    public sealed class SessionManager
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;

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
        /// Get hibernate configuration
        /// </summary>
        /// <returns><see cref="Configuration"/></returns>
        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new CfgSerialization("cfg.thunder").Load();

                    if (_configuration == null)
                        throw new InvalidOperationException("NHibernate configuration is null.");
                }
                
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