using System.Web.Mvc;
using NUnit.Framework;

namespace Thunder.Web.Mvc.Extensions
{
    [TestFixture]
    public class ModelStateDictionaryExtensionsTest
    {
        private static ModelStateDictionary CreateInvalidModelState()
        {
            var modelState = new ModelStateDictionary();

            modelState.AddModelError("Endereco.Rua", "Rua é obrigatória");
            modelState.AddModelError("Endereco.Numero", "Número é obrigatório");
            modelState.AddModelError("Nome", "Nome é obrigatório");

            return modelState;
        }

        [Test]
        public void ExcludePropertiesWithKeyPart_SemIgnoreKeys_RemoveTodasAsChavesComOKeyPart()
        {
            var modelState = CreateInvalidModelState();

            modelState.ExcludePropertiesWithKeyPart("Endereco", null);

            Assert.IsFalse(modelState.ContainsKey("Endereco.Rua"));
            Assert.IsFalse(modelState.ContainsKey("Endereco.Numero"));
            Assert.IsTrue(modelState.ContainsKey("Nome"));
        }

        [Test]
        public void ExcludePropertiesWithKeyPart_ComIgnoreKeys_PreservaChaveIgnorada()
        {
            var modelState = CreateInvalidModelState();

            modelState.ExcludePropertiesWithKeyPart("Endereco", new[] { "Endereco.Numero" });

            Assert.IsFalse(modelState.ContainsKey("Endereco.Rua"));
            Assert.IsTrue(modelState.ContainsKey("Endereco.Numero"));
            Assert.IsTrue(modelState.ContainsKey("Nome"));
        }
    }
}
