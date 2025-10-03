using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Persistencia;
using FluentAssertions;
using Xunit;

namespace Tests.Unit.Dominio
{
    public class MotoTests
    {
        [Fact]
        public void Construtor_ComParametrosValidos_DeveCriarMoto()
        {
            // Arrange
            var placa = "ABC1234";
            var modelo = (ModeloMotoEnum)1;
            var idPatio = 1;
            var chassi = "1HGBH41JXMN109186";
            var patio = new Patio("Patio Teste", "Endereco Teste");
            var idCarrapato = 1;

            // Act
            var moto = new Moto(placa, modelo, idPatio, chassi, patio, idCarrapato);

            // Assert
            moto.Placa.Should().Be("ABC1234");
            moto.Modelo.Should().Be(modelo);
            moto.IdPatio.Should().Be(idPatio);
            moto.Chassi.Should().Be("1HGBH41JXMN109186");
            moto.Zona.Should().Be(ZonaEnum.Saguao);
            moto.IdCarrapato.Should().Be(idCarrapato);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Construtor_ComPlacaInvalida_DeveLancarExcecao(string placaInvalida)
        {
            // Arrange
            var modelo = (ModeloMotoEnum)1;
            var idPatio = 1;
            var chassi = "1HGBH41JXMN109186";
            var patio = new Patio("Patio Teste", "Endereco Teste");
            var idCarrapato = 1;

            // Act & Assert
            Action act = () => new Moto(placaInvalida, modelo, idPatio, chassi, patio, idCarrapato);
            act.Should().Throw<ExcecaoDominio>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("123456789012345")] // 15 caracteres
        [InlineData("123456789012345678")] // 18 caracteres
        public void Construtor_ComChassiInvalido_DeveLancarExcecao(string chassiInvalido)
        {
            // Arrange
            var placa = "ABC1234";
            var modelo = (ModeloMotoEnum)1;
            var idPatio = 1;
            var patio = new Patio("Patio Teste", "Endereco Teste");
            var idCarrapato = 1;

            // Act & Assert
            Action act = () => new Moto(placa, modelo, idPatio, chassiInvalido, patio, idCarrapato);
            act.Should().Throw<ExcecaoDominio>();
        }

        [Fact]
        public void Construtor_DeveTornarPlacaEChassiMaiusculos()
        {
            // Arrange
            var placa = "abc1234";
            var modelo = (ModeloMotoEnum)1;
            var idPatio = 1;
            var chassi = "1hgbh41jxmn109186";
            var patio = new Patio("Patio Teste", "Endereco Teste");
            var idCarrapato = 1;

            // Act
            var moto = new Moto(placa, modelo, idPatio, chassi, patio, idCarrapato);

            // Assert
            moto.Placa.Should().Be("ABC1234");
            moto.Chassi.Should().Be("1HGBH41JXMN109186");
        }
    }
}
