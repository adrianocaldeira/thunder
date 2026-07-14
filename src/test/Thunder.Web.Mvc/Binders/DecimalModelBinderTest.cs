using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Thunder.Web.Mvc.Binders;

namespace Thunder.Web.Mvc.Binders
{
    [TestFixture]
    public class DecimalModelBinderTest
    {
        private const string ModelName = "Valor";

        private static ModelBindingContext CreateBindingContext(IValueProvider valueProvider)
        {
            return new ModelBindingContext
            {
                ModelName = ModelName,
                ValueProvider = valueProvider,
                ModelState = new ModelStateDictionary()
            };
        }

        [Test]
        public void BindModel_CampoNaoPostado_RetornaNullSemLancarException()
        {
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(v => v.GetValue(ModelName)).Returns((ValueProviderResult)null);

            var bindingContext = CreateBindingContext(valueProviderMock.Object);
            var binder = new DecimalModelBinder();

            object result = null;

            Assert.DoesNotThrow(() => result = binder.BindModel(null, bindingContext));
            Assert.IsNull(result);
        }

        [Test]
        public void BindModel_ValorComSeparadorDeMilharEmCulturaPtBr_RetornaDecimalCorreto()
        {
            var culturaOriginal = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");

                var valueProviderMock = new Mock<IValueProvider>();
                valueProviderMock.Setup(v => v.GetValue(ModelName))
                    .Returns(new ValueProviderResult("1.234,56", "1.234,56", CultureInfo.CurrentCulture));

                var bindingContext = CreateBindingContext(valueProviderMock.Object);
                var binder = new DecimalModelBinder();

                var result = binder.BindModel(null, bindingContext);

                Assert.AreEqual(1234.56m, result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = culturaOriginal;
            }
        }

        [Test]
        public void BindModel_ValorMaiorQueDecimalMaxValue_RetornaNullEAdicionaErroNoModelStateSemLancarException()
        {
            const string valorExtrapolado = "99999999999999999999999999999999999999";

            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(v => v.GetValue(ModelName))
                .Returns(new ValueProviderResult(valorExtrapolado, valorExtrapolado, CultureInfo.CurrentCulture));

            var bindingContext = CreateBindingContext(valueProviderMock.Object);
            var binder = new DecimalModelBinder();

            object result = null;

            Assert.DoesNotThrow(() => result = binder.BindModel(null, bindingContext));
            Assert.IsNull(result);
            Assert.Greater(bindingContext.ModelState[ModelName].Errors.Count, 0);
        }

        [Test]
        public void BindModel_StringVazia_RetornaNull()
        {
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(v => v.GetValue(ModelName))
                .Returns(new ValueProviderResult(string.Empty, string.Empty, CultureInfo.CurrentCulture));

            var bindingContext = CreateBindingContext(valueProviderMock.Object);
            var binder = new DecimalModelBinder();

            var result = binder.BindModel(null, bindingContext);

            Assert.IsNull(result);
        }
    }
}
