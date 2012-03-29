using System;
using NHibernate;
using NHibernate.Cfg;

namespace Thunder.NHibernate
{
    /// <summary>
    /// Hibernate session manager
    /// </summary>
    public sealed class SessionManager
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly Configuration _configuration;

        /// <summary>
        /// Initialize new instance of <see cref="SessionManager"/>.
        /// </summary>
        private SessionManager()
        {
            if (_sessionFactory == null)
            {
                var serialized = new CfgSerialization("cfg.thunder");
                _configuration = serialized.Load();


                if (_configuration == null)
                    throw new InvalidOperationException("NHibernate configuration is null.");

                _sessionFactory = _configuration.BuildSessionFactory();

                if (_sessionFactory == null)
                    throw new InvalidOperationException("Call to configuration.BuildSessionFactory() returned null.");            
            }
        }

        /// <summary>
        /// Get instance of <see cref="SessionManager"/>.
        /// </summary>
        public static SessionManager Instance
        {
            get { return NestedSessionManager.SessionManager; }
        }

        /// <summary>
        /// Get current instance of <see cref="ISessionFactory"/>.
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get { return Instance._sessionFactory; }
        }

        /// <summary>
        /// Get instance of <see cref="ISessionFactory"/>.
        /// </summary>
        /// <returns></returns>
        private ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        /// <summary>
        /// Get current session opened 
        /// </summary>
        /// <returns><see cref="ISession"/></returns>
        public static ISession OpenSession()
        {
            return Instance.GetSessionFactory().OpenSession();
        }

        /// <summary>
        /// Get hibernate configuration
        /// </summary>
        /// <returns><see cref="Configuration"/></returns>
        public static Configuration Configuration()
        {
            return Instance._configuration;
        }

        /// <summary>
        /// Internal nested session manager
        /// </summary>
        internal class NestedSessionManager
        {
            internal static readonly SessionManager SessionManager = new SessionManager();
        }
    }
}