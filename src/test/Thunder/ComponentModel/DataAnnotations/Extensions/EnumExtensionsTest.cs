using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Thunder.ComponentModel.DataAnnotations.Extensions
{
    [TestFixture]
    public class EnumExtensionsTest
    {
        private enum WithoutDisplay
        {
            FirstValue,
            SecondValue
        }

        private enum WithDisplay
        {
            [Display(Name = "Primeiro valor")]
            FirstValue
        }

        [Test]
        public void DisplayName_SemAtributoDisplay_RetornaNomeDoMembro()
        {
            Assert.AreEqual("FirstValue", WithoutDisplay.FirstValue.DisplayName());
            Assert.AreEqual("SecondValue", WithoutDisplay.SecondValue.DisplayName());
        }

        [Test]
        public void DisplayName_ComAtributoDisplay_RetornaNomeConfigurado()
        {
            Assert.AreEqual("Primeiro valor", WithDisplay.FirstValue.DisplayName());
        }
    }
}
