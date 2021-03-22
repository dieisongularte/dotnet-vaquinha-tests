using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using Vaquinha.Tests.Common.Fixtures;
using Xunit;

namespace Vaquinha.AutomatedUITests
{
	public class DoacaoTests : IDisposable, IClassFixture<DoacaoFixture>, 
                                               IClassFixture<EnderecoFixture>, 
                                               IClassFixture<CartaoCreditoFixture>
	{
		private DriverFactory _driverFactory = new DriverFactory();
		private IWebDriver _driver;

		private readonly DoacaoFixture _doacaoFixture;
		private readonly EnderecoFixture _enderecoFixture;
		private readonly CartaoCreditoFixture _cartaoCreditoFixture;

		public DoacaoTests(DoacaoFixture doacaoFixture, EnderecoFixture enderecoFixture, CartaoCreditoFixture cartaoCreditoFixture)
        {
            _doacaoFixture = doacaoFixture;
            _enderecoFixture = enderecoFixture;
            _cartaoCreditoFixture = cartaoCreditoFixture;
        }
		public void Dispose()
		{
			_driverFactory.Close();
		}

		[Fact]
		public void DoacaoUI_AcessoTelaHome()
		{
			// Arrange
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			// Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("vaquinha-logo"));

			// Assert
			webElement.Displayed.Should().BeTrue(because:"logo exibido");
		}
		[Fact]
		public void DoacaoUI_CriacaoDoacao()
		{
			//Arrange
			var doacao = _doacaoFixture.DoacaoValida();
            doacao.AdicionarEnderecoCobranca(_enderecoFixture.EnderecoValido());
            doacao.AdicionarFormaPagamento(_cartaoCreditoFixture.CartaoCreditoValido());
			_driverFactory.NavigateToUrl("https://vaquinha.azurewebsites.net/");
			_driver = _driverFactory.GetWebDriver();

			//Act
			IWebElement webElement = null;
			webElement = _driver.FindElement(By.ClassName("btn-yellow"));
			webElement.Click();

			IWebElement campoNome = _driver.FindElement(By.Id("DadosPessoais_Nome"));
			campoNome.SendKeys(doacao.DadosPessoais.Nome);

			IWebElement campoEmail = _driver.FindElement(By.Id("DadosPessoais_Email"));
			campoEmail.SendKeys(doacao.DadosPessoais.Email);

			IWebElement cep = _driver.FindElement(By.Id("cep"));
			cep.SendKeys(doacao.EnderecoCobranca.CEP);

			IWebElement endereco = _driver.FindElement(By.Id("EnderecoCobranca_TextoEndereco"));
			endereco.SendKeys(doacao.EnderecoCobranca.TextoEndereco);

			IWebElement numero = _driver.FindElement(By.Id("EnderecoCobranca_Numero"));
			numero.SendKeys(doacao.EnderecoCobranca.Numero);

			IWebElement cidade = _driver.FindElement(By.Id("EnderecoCobranca_Cidade"));
			cidade.SendKeys(doacao.EnderecoCobranca.Cidade);

			IWebElement telefone = _driver.FindElement(By.Id("telefone"));
			telefone.SendKeys(doacao.EnderecoCobranca.Telefone);

			IWebElement titular = _driver.FindElement(By.Id("FormaPagamento_NomeTitular"));
			titular.SendKeys(doacao.FormaPagamento.NomeTitular);
			
			IWebElement cardNumber = _driver.FindElement(By.Id("cardNumber"));
			cardNumber.SendKeys(doacao.FormaPagamento.NumeroCartaoCredito);

			IWebElement cvv = _driver.FindElement(By.Id("cvv"));
			cvv.SendKeys(doacao.FormaPagamento.CVV);

			IWebElement validade = _driver.FindElement(By.Id("validade"));
			validade.SendKeys(doacao.FormaPagamento.Validade);

			IWebElement btnDoar = _driver.FindElement(By.ClassName("btn-yellow"));
			btnDoar.Click();

			//Assert
			_driver.Url.Should().Contain("/Home/Index");
		}
	}
}