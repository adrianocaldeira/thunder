using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NHibernate;
using NHibernate.Context;
using NHibernate.Event;
using Configuration = NHibernate.Cfg.Configuration;

namespace Thunder.Data
{
    /// <summary>
    ///     NHibernate Session Manager
    /// </summary>
    public sealed class SessionManager
    {
        private static Configuration _configuration;
        private static ISessionFactory _sessionFactory;
        private static bool? _serializeConfiguration;
        private static readonly object _lockObject = new object();

        /// <summary>
        ///     Get current instance of <see cref="ISessionFactory" />.
        /// </summary>
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory != null) return _sessionFactory;

                lock (_lockObject)
                {
                    _sessionFactory = Configuration.BuildSessionFactory();

                    if (_sessionFactory == null)
                        throw new InvalidOperationException("Call to configuration.BuildSessionFactory() returned null.");

                    return _sessionFactory;
                }
            }
        }

        /// <summary>
        ///     Get if serialize configuration
        /// </summary>
        public static bool SerializeConfiguration
        {
            get
            {
                if (_serializeConfiguration != null) return _serializeConfiguration.Value;

                try
                {
                    _serializeConfiguration =
                        !string.IsNullOrEmpty(
                            ConfigurationManager.AppSettings["Thunder.Data.SessionManager.SerializeConfiguration"]) &&
                        Convert.ToBoolean(
                            ConfigurationManager.AppSettings["Thunder.Data.SessionManager.SerializeConfiguration"]);
                }
                catch (Exception)
                {
                    _serializeConfiguration = false;
                }

                return _serializeConfiguration.Value;
            }
        }

        /// <summary>
        ///     Get or set listeners
        /// </summary>
        public static Dictionary<ListenerType, object[]> Listeners { get; set; }

        /// <summary>
        ///     Get hibernate configuration
        /// </summary>
        /// <returns>
        ///     <see cref="Configuration" />
        /// </returns>
        public static Configuration Configuration
        {
            get
            {
                if (_configuration != null) return _configuration;

                lock (_lockObject)
                {
                    _configuration = SerializeConfiguration
                        ? new CfgSerialization("cfg.thunder").Load()
                        : new Configuration();

                    if (_configuration == null)
                        throw new InvalidOperationException("NHibernate configuration is null.");

                    if (Listeners != null && Listeners.Any())
                    {
                        foreach (var listener in Listeners)
                        {
                            _configuration.SetListeners(listener.Key, listener.Value);
                        }
                    }

                    _configuration.Configure();

                    return _configuration;
                }
            }
        }

        /// <summary>
        ///     Get current session
        /// </summary>
        public static ISession CurrentSession
        {
            get
            {
                if (!CurrentSessionContext.HasBind(SessionFactory))
                {
                    CurrentSessionContext.Bind(SessionFactory.OpenSession());
                }

                return SessionFactory.GetCurrentSession();
            }
        }

        /// <summary>
        ///     Bind nhibernate session
        /// </summary>
        public static void Bind()
        {
            CurrentSessionContext.Bind(SessionFactory.OpenSession());
        }

        /// <summary>
        ///     Unbind nhibernate session
        /// </summary>
        public static void Unbind()
        {
            lock (_lockObject)
            {
                if (CurrentSessionContext.HasBind(SessionFactory))
                {
                    CurrentSessionContext.Unbind(SessionFactory).Dispose();
                }
            }
        }
    }
}