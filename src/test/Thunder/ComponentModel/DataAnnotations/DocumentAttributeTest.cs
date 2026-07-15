using NUnit.Framework;

namespace Thunder.ComponentModel.DataAnnotations
{
    [TestFixture]
    class DocumentAttributeTest
    {
        [Test]
        public void Cnpj_Valido()
        {
            var attribute = new DocumentAttribute {Type = DocumentType.Cnpj};

            Assert.IsTrue(attribute.IsValid("12.ABC.345/0001-88")); // alfanumérico com máscara
            Assert.IsTrue(attribute.IsValid("12ABC345000188"));     // alfanumérico sem máscara
            Assert.IsTrue(attribute.IsValid("33846757000150"));     // numérico (retrocompat)
        }

        [Test]
        public void Cnpj_Invalido()
        {
            var attribute = new DocumentAttribute {Type = DocumentType.Cnpj};

            Assert.IsFalse(attribute.IsValid("12ABC345000199")); // dígito verificador errado
        }

        [Test]
        public void Cpf_Valido()
        {
            var attribute = new DocumentAttribute {Type = DocumentType.Cpf};

            Assert.IsTrue(attribute.IsValid("767.866.253-04"));
        }

        [Test]
        public void Valor_Nulo_EhValido()
        {
            var attribute = new DocumentAttribute {Type = DocumentType.Cnpj};

            // null é considerado válido pelo atributo (a obrigatoriedade fica a cargo de [Required]).
            Assert.IsTrue(attribute.IsValid(null));
        }
    }
}
