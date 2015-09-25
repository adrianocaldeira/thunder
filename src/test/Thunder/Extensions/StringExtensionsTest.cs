using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Thunder.Extensions
{
    [TestFixture]
    class StringExtensionsTest
    {
        [Test]
        public void RemoveLastCaracter()
        {
            Assert.AreEqual("test", "test,".RemoveLastCaracter(","));
            Assert.AreEqual("test", "test".RemoveLastCaracter(","));
        }

        [Test]
        public void TextIfEmpty()
        {
            Assert.AreEqual("Empty", "".TextIfEmpty("Empty"));
            Assert.AreEqual("Test", "Test".TextIfEmpty("Empty"));
        }

        [Test]
        public void IsCpf()
        {
            Assert.IsTrue("767.866.253-04".IsCpf());
            Assert.IsTrue("76786625304".IsCpf());
            Assert.IsFalse("".IsCpf());
            Assert.IsFalse("76786625301".IsCpf());
        }


        [Test]
        public void IsCnpj()
        {
            Assert.IsTrue("33.846.757/0001-50".IsCnpj());
            Assert.IsTrue("33846757000150".IsCnpj());
            Assert.IsFalse("".IsCnpj());
            Assert.IsFalse("33846757000151".IsCnpj());
        }

        [Test]
        public void IsEmail()
        {
            Assert.IsTrue("adriano@integgro.com.br".IsEmail());
            Assert.IsTrue("adriano@integgro.com".IsEmail());
            Assert.IsTrue("adriano_@integgro.com".IsEmail());
            Assert.IsTrue("adriano_caldeira@integgro.com".IsEmail());
            Assert.IsTrue("adriano.caldeira@integgro.com".IsEmail());
            Assert.IsFalse("adriano . caldeira@integgro.com".IsEmail());
            Assert.IsFalse("adriano . caldeira@integgro".IsEmail());
            Assert.IsFalse("adriano.caldeira@integgro".IsEmail());
            Assert.IsFalse("adriano@integgro".IsEmail());
            Assert.IsFalse("".IsEmail());
        }

        [Test]
        public void IsDate()
        {
            Assert.IsTrue("01/01/1901".IsDate());
            Assert.IsTrue("20/01/1901".IsDate());
            Assert.IsTrue("01/01/1001".IsDate());
            Assert.IsFalse("01011001".IsDate());
            Assert.IsFalse("".IsDate());
        }

        [Test]
        public void IsHour()
        {
            Assert.IsTrue("09:13".IsHour());
            Assert.IsTrue("22:35".IsHour());
            Assert.IsTrue("23:59".IsHour());
            Assert.IsTrue("00:00".IsHour());
            Assert.IsFalse("24:00".IsHour());
            Assert.IsFalse("23:61".IsHour());
            Assert.IsFalse("".IsHour());
        }

        [Test]
        public void IsUrl()
        {
            Assert.IsTrue("wwww.integgro.com.br".IsUrl(false));
            Assert.IsTrue("http://wwww.integgro.com.br".IsUrl());
            Assert.IsTrue("https://wwww.integgro.com.br".IsUrl());
            Assert.IsTrue("ftp://wwww.integgro.com.br".IsUrl());
            Assert.IsFalse("".IsUrl());
            Assert.IsFalse("".IsUrl(false));
            Assert.IsFalse("integgro.com.br".IsUrl());
        }

        [Test]
        public void Reduce()
        {
            Assert.AreEqual("My...", "My test. My fast test.".Reduce(5, "..."));
            Assert.AreEqual("My test. My fast test.", "My test. My fast test.".Reduce(100, "..."));
        }

        [Test]
        public void RemoveSpaces()
        {
            Assert.AreEqual("Mytest", "My test ".RemoveSpaces());
        }

        [Test]
        public void Truncate()
        {
            Assert.AreEqual("My te", "My test. My fast test.".Truncate(5));
            Assert.AreEqual("My test. My fast test.", "My test. My fast test.".Truncate(100));
        }

        [Test]
        public void IsEmpty()
        {
            Assert.IsTrue("".IsEmpty());
            Assert.IsTrue(" ".IsEmpty());
            Assert.IsFalse("test".IsEmpty());
        }

        [Test]
        public void With()
        {
            Assert.AreEqual("test", "t{0}s{1}".With("e","t"));
        }

        [Test]
        public void Join()
        {
            Assert.AreEqual("Adriano-Herminio-Caldeira", new List<string>{"Adriano","Herminio", "Caldeira"}.Join("-"));
            Assert.AreEqual("Adriano-Herminio-Caldeira", new object[] { "Adriano", "Herminio", "Caldeira" }.Join("-"));
        }

        [Test]
        public void IsNumberOnly()
        {
            Assert.IsTrue("1291212".IsNumberOnly(false));
            Assert.IsTrue("12912.12".IsNumberOnly(true));
            Assert.IsFalse("".IsNumberOnly(true));
            Assert.IsFalse("".IsNumberOnly(false));
            Assert.IsFalse("a".IsNumberOnly(true));
            Assert.IsFalse("a".IsNumberOnly(false));
        }

        [Test]
        public void NlToBr()
        {
            var builder = new StringBuilder();
            builder.Append("Test 1")
                .AppendLine()
                .Append("Test 2");

            Assert.AreEqual("Test 1<br />Test 2", builder.ToString().NlToBr());
        }

        [Test]
        public void RemoveAccent()
        {
            Assert.AreEqual("aaaaaceeeeiiiiooooouuuuAAAAACEEEEIIIIOOOOOUUUU", "àáäâãçèéëêìíïîòóöôõùúüûÀÁÄÂÃÇÈÉËÊÌÍÏÎÒÓÖÔÕÙÚÜÛ".RemoveAccent());
        }

        [Test]
        public void ToSeo()
        {
            Assert.AreEqual("cabeca-de-dinossauro-foi-achada-no-chao-de-um-parque-aquatico", "cabeça de    dinossauro & foi achada no chão de um parque aquático".ToSeo());
            Assert.AreEqual("cabeca-de-dinossauro-foi-achada-no-chao", "cabeça de    dinossauro & foi achada no chão de um parque aquático".ToSeo(40));
        }

        [Test]
        public void ToHash()
        {
            Assert.AreEqual(3406283506426094898, "cabeca-de-dinossauro-foi-achada-no-chao-de-um-parque-aquatico".ToHash());
        }

        [Test]
        public void Format()
        {
            Assert.AreEqual("537.783.216-76", "53778321676".Format(FormatType.Cpf));
            Assert.AreEqual("53.823.456/0001-18", "53823456000118".Format(FormatType.Cnpj));
            Assert.AreEqual("03068-090", "03068090".Format(FormatType.ZipCode));
            Assert.AreEqual("(11) 97587-5558", "11975875558".Format(FormatType.Phone));
            Assert.AreEqual("(11) 7587-5558", "1175875558".Format(FormatType.Phone));
        }

        [Test]
        public void IsPhone()
        {
            Assert.IsFalse("(11)1111-1111".IsPhone());
            Assert.IsFalse("(11)11111-1111".IsPhone());
            Assert.IsFalse("(11)0111-1111".IsPhone());
            Assert.IsFalse("(11)01111-1111".IsPhone());
            Assert.IsFalse("(00)11111-1111".IsPhone());
            Assert.IsFalse("(01)11111-1111".IsPhone());
            Assert.IsFalse("(20) 3111-1112".IsPhone());
            Assert.IsFalse("(30) 3111-1112".IsPhone());
            Assert.IsFalse("(40) 3111-1112".IsPhone());
            Assert.IsFalse("(50) 3111-1112".IsPhone());
            Assert.IsFalse("(60) 3111-1112".IsPhone());
            Assert.IsFalse("(70) 3111-1112".IsPhone());
            Assert.IsFalse("(80) 3111-1112".IsPhone());
            Assert.IsFalse("(90) 3111-1112".IsPhone());
            Assert.IsFalse("(11) 31-1112".IsPhone());

            Assert.IsTrue("(10) 3111-1112".IsPhone());
            Assert.IsTrue("(11) 3111-1112".IsPhone());
            Assert.IsTrue("(11) 92111-1112".IsPhone());
            Assert.IsTrue("(32) 79111-1112".IsPhone());
            Assert.IsTrue("(99) 2111-1112".IsPhone());
            Assert.IsTrue("(11) 2222-2222".IsPhone());
        }

        [Test]
        public void IsZipCode()
        {
            Assert.IsFalse("00000-000".IsZipCode());
            Assert.IsFalse("11111-111".IsZipCode());
            Assert.IsFalse("22222-222".IsZipCode());
            Assert.IsFalse("33333-333".IsZipCode());
            Assert.IsFalse("44444-444".IsZipCode());
            Assert.IsFalse("55555-555".IsZipCode());
            Assert.IsFalse("66666-666".IsZipCode());
            Assert.IsFalse("77777-777".IsZipCode());
            Assert.IsFalse("88888-888".IsZipCode());
            Assert.IsFalse("99999-999".IsZipCode());
            Assert.IsTrue("03067-000".IsZipCode());
        }
    }
}
