using System;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace Thunder.Extensions
{
    [TestFixture]
    public class ObjectExtensionsTest
    {
        [Test]
        public void Cast_StringComPontoDecimal_ConverteComoInvarianteIndependenteDaCulturaCorrente()
        {
            var originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Assert.AreEqual(1.5m, "1.5".Cast<decimal>());
                Assert.AreEqual(1.5d, "1.5".Cast<double>());

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Assert.AreEqual(1.5m, "1.5".Cast<decimal>());
                Assert.AreEqual(1.5d, "1.5".Cast<double>());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Test]
        public void Cast_StringComVirgula_TrataComoSeparadorDeMilharIndependenteDaCulturaCorrente()
        {
            // Com cultura invariante seca, a vírgula é o separador de milhar
            // do invariant (não o separador decimal do pt-BR) — "1,5" vira 15m
            // deterministicamente, seja qual for a cultura corrente da thread.
            var originalCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                Assert.AreEqual(15m, "1,5".Cast<decimal>());

                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Assert.AreEqual(15m, "1,5".Cast<decimal>());
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }
    }
}
