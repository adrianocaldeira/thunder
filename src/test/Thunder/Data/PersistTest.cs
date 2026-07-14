using System;
using NUnit.Framework;
using Thunder.Data;

namespace Thunder.Data
{
    [TestFixture]
    class PersistTest
    {
        class IntEntity : Persist<IntEntity, int> { }
        class GuidEntity : Persist<GuidEntity, Guid> { }
        class StringEntity : Persist<StringEntity, string> { }

        [Test]
        public void IsNew_int_zero_e_negativo()
        {
            Assert.IsTrue(new IntEntity { Id = 0 }.IsNew());
            Assert.IsFalse(new IntEntity { Id = 1 }.IsNew());
        }

        [Test]
        public void IsNew_guid_empty()
        {
            Assert.IsTrue(new GuidEntity { Id = Guid.Empty }.IsNew());
            Assert.IsFalse(new GuidEntity { Id = Guid.NewGuid() }.IsNew());
        }

        [Test]
        public void IsNew_string_null_nao_explode()
        {
            Assert.IsTrue(new StringEntity { Id = null }.IsNew());
            Assert.IsFalse(new StringEntity { Id = "ABC" }.IsNew());
        }

        [Test]
        public void Equals_por_id_e_tipo()
        {
            var a = new IntEntity { Id = 5 };
            var b = new IntEntity { Id = 5 };
            var c = new IntEntity { Id = 6 };
            Assert.IsTrue(a.Equals(b));
            Assert.IsFalse(a.Equals(c));
        }

        [Test]
        public void Equals_transientes_so_por_referencia()
        {
            var a = new IntEntity { Id = 0 };
            var b = new IntEntity { Id = 0 };
            Assert.IsFalse(a.Equals(b));          // ambos novos → não iguais
            Assert.IsTrue(a.Equals(a));
        }

        [Test]
        public void GetHashCode_estavel_apos_atribuir_id()
        {
            var e = new IntEntity();              // transiente
            var h1 = e.GetHashCode();
            e.Id = 42;                            // persistido depois
            Assert.AreEqual(h1, e.GetHashCode()); // hash não muda
        }

        [Test]
        public void GetHashCode_nao_explode_com_id_nulo()
        {
            Assert.DoesNotThrow(() => new StringEntity { Id = null }.GetHashCode());
        }
    }
}
