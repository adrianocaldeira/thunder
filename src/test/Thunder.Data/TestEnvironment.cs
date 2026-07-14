using System.Collections.Generic;
using NHibernate.Event;
using NUnit.Framework;
using Thunder.NHibernate;
using Thunder.NHibernate.Pattern;

namespace Thunder.Data
{
    /// <summary>
    ///     Bootstrap global dos testes: registra os listeners no <see cref="SessionManager" />
    ///     antes de qualquer fixture executar — e, portanto, antes do primeiro acesso a
    ///     <see cref="SessionManager.Configuration" />/<see cref="SessionManager.SessionFactory" />,
    ///     que materializam a configuração uma única vez por processo.
    /// </summary>
    [SetUpFixture]
    public class TestEnvironment
    {
        /// <summary>
        ///     Registra o <see cref="CreatedAndUpdatedPropertyEventListener" /> para os eventos
        ///     de pré-inserção e pré-atualização.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var listener = new CreatedAndUpdatedPropertyEventListener();

            // Os arrays precisam ser tipados por evento: Configuration.SetListeners faz cast
            // direto do array recebido para o tipo do listener correspondente.
            SessionManager.Listeners = new Dictionary<ListenerType, object[]>
            {
                { ListenerType.PreInsert, new IPreInsertEventListener[] { listener } },
                { ListenerType.PreUpdate, new IPreUpdateEventListener[] { listener } }
            };
        }
    }
}
