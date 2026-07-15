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
    /// <remarks>
    ///     Uma falha na primeira materialização da configuração ou da fábrica de sessões (por exemplo,
    ///     banco de dados indisponível durante a inicialização da aplicação) NÃO é permanente: a exceção
    ///     é propagada ao chamador, mas o próximo acesso tenta materializar novamente. Uma vez
    ///     materializadas com sucesso, as instâncias permanecem as mesmas durante toda a vida do processo.
    /// </remarks>
    public sealed class SessionManager
    {
        private static Lazy<Configuration> _configuration = NewConfigurationLazy();

        private static Lazy<ISessionFactory> _sessionFactory = NewSessionFactoryLazy();

        private static Lazy<Configuration> NewConfigurationLazy() =>
            new Lazy<Configuration>(BuildConfiguration, LazyThreadSafetyMode.ExecutionAndPublication);

        private static Lazy<ISessionFactory> NewSessionFactoryLazy() =>
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
        /// <remarks>
        ///     Se a construção falhar, a exceção é propagada e o próximo acesso tenta novamente
        ///     (a falha não fica cacheada de forma permanente).
        /// </remarks>
        /// <returns>
        ///     <see cref="Configuration" />
        /// </returns>
        public static Configuration Configuration => Materialize(ref _configuration, NewConfigurationLazy);

        /// <summary>
        ///     Obtém a instância única de <see cref="ISessionFactory" />.
        /// </summary>
        /// <remarks>
        ///     Se a construção falhar, a exceção é propagada e o próximo acesso tenta novamente
        ///     (a falha não fica cacheada de forma permanente).
        /// </remarks>
        public static ISessionFactory SessionFactory => Materialize(ref _sessionFactory, NewSessionFactoryLazy);

        private static T Materialize<T>(ref Lazy<T> slot, Func<Lazy<T>> factory)
        {
            var current = slot;
            try
            {
                return current.Value;
            }
            catch
            {
                // Falha transitória (ex.: banco indisponível no boot): descarta o Lazy que
                // cacheou a exceção para permitir nova tentativa num próximo acesso, sem
                // perder a thread-safety (só troca se ninguém já substituiu o slot).
                Interlocked.CompareExchange(ref slot, factory(), current);
                throw;
            }
        }

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
