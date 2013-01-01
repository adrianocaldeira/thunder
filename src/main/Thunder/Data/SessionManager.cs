using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace Thunder.Data
{
    /// <summary>
    /// Hibernate session manager
    /// </summary>
    public sealed class SessionManager
    {
        private readonly Configuration _configuration;
        private readonly ISessionFactory _sessionFactory;

        /// <summary>
        /// Initialize new instance of <see cref="SessionManager"/>.
        /// </summary>
        private SessionManager()
        {
            if (_sessionFactory != null) return;

            var serialized = new CfgSerialization("cfg.thunder");
            _configuration = serialized.Load();

            if (_configuration == null)
                throw new InvalidOperationException("NHibernate configuration is null.");

            _sessionFactory = _configuration.BuildSessionFactory();

            if (_sessionFactory == null)
                throw new InvalidOperationException("Call to configuration.BuildSessionFactory() returned null.");
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
        /// Get hibernate configuration
        /// </summary>
        /// <returns><see cref="Configuration"/></returns>
        public static Configuration Configuration
        {
            get { return Instance._configuration; }
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

        #region Nested type: NestedSessionManager

        /// <summary>
        /// Internal nested session manager
        /// </summary>
        internal class NestedSessionManager
        {
            internal static readonly SessionManager SessionManager = new SessionManager();
        }

        #endregion
    }
}