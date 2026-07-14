using System;
using System.Collections.Generic;
using System.Threading;
using NHibernate;
using NHibernate.Context;
using NHibernate.Event;
using Configuration = NHibernate.Cfg.Configuration;

namespace Thunder.NHibernate
{
    /// <summary>
    ///     Gerenciador de sessões do NHibernate: mantém uma única <see cref="Configuration" /> e uma única
    ///     <see cref="ISessionFactory" /> por processo, inicializadas de forma tardia e thread-safe.
    /// </summary>
    public sealed class SessionManager
    {
        private static readonly Lazy<Configuration> _configuration =
            new Lazy<Configuration>(BuildConfiguration, LazyThreadSafetyMode.ExecutionAndPublication);

        private static readonly Lazy<ISessionFactory> _sessionFactory =
            new Lazy<ISessionFactory>(() => Configuration.BuildSessionFactory(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        ///     Obtém ou define os listeners aplicados à <see cref="Configuration" />.
        /// </summary>
        /// <remarks>
        ///     Devem ser definidos antes do primeiro acesso a <see cref="Configuration" /> ou
        ///     <see cref="SessionFactory" />; após a materialização da configuração, alterações
        ///     nesta propriedade não têm efeito.
        /// </remarks>
        public static Dictionary<ListenerType, object[]> Listeners { get; set; }

        /// <summary>
        ///     Obtém a configuração do NHibernate, criada uma única vez por processo.
        /// </summary>
        /// <returns>
        ///     <see cref="Configuration" />
        /// </returns>
        public static Configuration Configuration => _configuration.Value;

        /// <summary>
        ///     Obtém a instância única de <see cref="ISessionFactory" />.
        /// </summary>
        public static ISessionFactory SessionFactory => _sessionFactory.Value;

        /// <summary>
        ///     Obtém a sessão corrente, abrindo e vinculando uma nova sessão ao contexto se necessário.
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
        ///     Abre uma nova sessão e a vincula ao contexto corrente do NHibernate.
        /// </summary>
        public static void Bind()
        {
            CurrentSessionContext.Bind(SessionFactory.OpenSession());
        }

        /// <summary>
        ///     Desvincula a sessão do contexto corrente do NHibernate e a descarta.
        /// </summary>
        public static void Unbind()
        {
            if (CurrentSessionContext.HasBind(SessionFactory))
            {
                CurrentSessionContext.Unbind(SessionFactory).Dispose();
            }
        }

        private static Configuration BuildConfiguration()
        {
            var configuration = new Configuration();

            if (Listeners != null)
            {
                foreach (var listener in Listeners)
                {
                    configuration.SetListeners(listener.Key, listener.Value);
                }
            }

            configuration.Configure();

            return configuration;
        }
    }
}
