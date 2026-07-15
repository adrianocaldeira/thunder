using System.Globalization;
using System.Threading;
using NUnit.Framework;

/// <summary>
/// Fixa a cultura pt-BR para toda a suíte de testes do assembly, garantindo
/// determinismo independente do locale do ambiente de execução (ex.: CI em en-US).
/// As validações e formatações brasileiras (datas dd/MM/yyyy, números, moeda)
/// assumem a cultura brasileira, que é a do ambiente de produção do consumidor.
/// </summary>
[SetUpFixture]
public class TestCultureSetup
{
    [OneTimeSetUp]
    public void FixarCulturaBrasileira()
    {
        var brasil = CultureInfo.GetCultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = brasil;
        CultureInfo.DefaultThreadCurrentUICulture = brasil;
        Thread.CurrentThread.CurrentCulture = brasil;
        Thread.CurrentThread.CurrentUICulture = brasil;
    }
}
