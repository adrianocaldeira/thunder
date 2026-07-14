using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using Thunder.Resources;

namespace Thunder.Web
{
    [TestFixture]
    class JsonExtensionsTest
    {
        [TearDown]
        public void TearDown()
        {
            JsonConvert.DefaultSettings = null;
            MaliciousGadget.Instantiated = false;
        }

        [Test]
        public void SerializeObject()
        {
            var json = new TireItem {Id = 1, State = ObjectState.Unchanged}.Json();
            Assert.AreEqual("{\"id\":1,\"state\":3}", json);
        }

        [Test]
        public void DeserializeObject()
        {
            var tireItem = "{\"Id\":1,\"State\":3}".Json<TireItem>();
            Assert.AreEqual(1, tireItem.Id);
            Assert.AreEqual(ObjectState.Unchanged, tireItem.State);
        }

        [Test]
        public void DeserializeObject_DoesNotInstantiateTypeFromMaliciousTypeNameEvenWithVulnerableGlobalDefaultSettings()
        {
            // Simula um app host que registrou DefaultSettings globais perigosos (ex.: TypeNameHandling.All),
            // algo fora do controle do Thunder. O overload padrão de Json<T>() precisa blindar contra isso.
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var json = "{\"$type\":\"Thunder.Web.MaliciousGadget, Thunder.Test\",\"Marker\":\"pwned\"}";

            json.Json<object>();

            Assert.IsFalse(MaliciousGadget.Instantiated,
                "TypeNameHandling herdado de DefaultSettings global não pode instanciar tipos arbitrários via $type");
        }

        [Test]
        public void DeserializeObject_ThrowsWhenJsonExceedsMaxDepthEvenWithVulnerableGlobalDefaultSettings()
        {
            // Simula um app host que removeu o limite de profundidade globalmente (MaxDepth = null).
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                MaxDepth = null
            };

            const int depth = 100;
            var builder = new StringBuilder();

            for (var i = 0; i < depth; i++)
            {
                builder.Append("{\"a\":");
            }

            builder.Append("1");

            for (var i = 0; i < depth; i++)
            {
                builder.Append("}");
            }

            Assert.Throws<JsonReaderException>(() => builder.ToString().Json<object>());
        }

    }

    /// <summary>
    ///     Tipo "gadget" usado apenas para provar, em teste, que um payload JSON com <c>$type</c>
    ///     malicioso não deve resultar em instanciação arbitrária de tipos.
    /// </summary>
    public class MaliciousGadget
    {
        public static bool Instantiated { get; set; }

        public string Marker { get; set; }

        public MaliciousGadget()
        {
            Instantiated = true;
        }
    }
}
