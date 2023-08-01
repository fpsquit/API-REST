
using APIREST.Helpers;

namespace ApiRestTests.MetodosHelpersTests
{

    [TestClass]
    public class PedidoHelperTests
    {

        [TestMethod]
        public void RemoverPontosCnpj_RetornaCnpjSemPontos()
        {
            // Arrange
            var cnpjComPontos = "70.734.571/0001-35";
            var cnpjSemPontosEsperado = "70734571000135";

            // Act
            var cnpjSemPontos = PedidoHelper.RemoverPontosCnpj(cnpjComPontos);

            // Assert
            Assert.AreEqual(cnpjSemPontosEsperado, cnpjSemPontos);
        }

    }
}