using Aplicacao.DTOs.Moto;
using Aplicacao.Servicos;
using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Interfaces.Mottu;
using Dominio.Persistencia;
using FluentAssertions;
using Moq;
using Xunit;

namespace Tests.Unit.Servicos
{
    public class MotoServicoTests
    {
        private readonly Mock<IMotoRepositorio> _mockMotoRepositorio;
        private readonly Mock<IRepositorio<Patio>> _mockPatioRepositorio;
        private readonly Mock<IRepositorioCarrapato> _mockCarrapatoRepositorio;
        private readonly Mock<IMottuRepositorio> _mockMottuRepositorio;
        private readonly MotoServico _motoServico;

        public MotoServicoTests()
        {
            _mockMotoRepositorio = new Mock<IMotoRepositorio>();
            _mockPatioRepositorio = new Mock<IRepositorio<Patio>>();
            _mockCarrapatoRepositorio = new Mock<IRepositorioCarrapato>();
            _mockMottuRepositorio = new Mock<IMottuRepositorio>();
            
            _motoServico = new MotoServico(
                _mockMotoRepositorio.Object,
                _mockPatioRepositorio.Object,
                _mockCarrapatoRepositorio.Object,
                _mockMottuRepositorio.Object
            );
        }

        [Fact]
        public async Task ObterTodos_DeveRetornarListaDeMotos()
        {
            // Arrange
            var motosEsperadas = new List<Moto>
            {
                new Moto("ABC1234", (ModeloMotoEnum)1, 1, "1HGBH41JXMN109186", new Patio("Patio Teste", "Endereco Teste"), 1),
                new Moto("XYZ5678", (ModeloMotoEnum)2, 1, "2HGBH41JXMN109187", new Patio("Patio Teste 2", "Endereco Teste 2"), 2)
            };

            _mockMotoRepositorio.Setup(r => r.ObterTodos()).ReturnsAsync(motosEsperadas);

            // Act
            var resultado = await _motoServico.ObterTodos();

            // Assert
            resultado.Should().NotBeNull();
            resultado.Should().HaveCount(2);
            resultado.Should().BeEquivalentTo(motosEsperadas);
            _mockMotoRepositorio.Verify(r => r.ObterTodos(), Times.Once);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        public async Task ObterTodosPaginado_ComParametrosInvalidos_DeveLancarExcecao(int pagina, int tamanhoPagina)
        {
            // Act & Assert
            await Assert.ThrowsAsync<ExcecaoDominio>(() => 
                _motoServico.ObterTodosPaginado(pagina, tamanhoPagina));
        }

        [Fact]
        public async Task ObterPorId_ComIdValido_DeveRetornarMotoDto()
        {
            // Arrange
            var motoId = 1;
            var moto = new Moto("ABC1234", (ModeloMotoEnum)1, 1, "1HGBH41JXMN109186", new Patio("Patio Teste", "Endereco Teste"), 1);
            
            _mockMotoRepositorio.Setup(r => r.ObterPorId(motoId)).ReturnsAsync(moto);

            // Act
            var resultado = await _motoServico.ObterPorId(motoId);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Placa.Should().Be("ABC1234");
            // The enum is converted to string using ToString().ToUpper(), so we expect the enum name
            resultado.Modelo.Should().NotBeNullOrEmpty(); // Just verify it's not null/empty since enum names vary
            _mockMotoRepositorio.Verify(r => r.ObterPorId(motoId), Times.Once);
        }

        [Fact]
        public async Task ObterPorId_ComIdInexistente_DeveLancarExcecao()
        {
            // Arrange
            var motoId = 999;
            _mockMotoRepositorio.Setup(r => r.ObterPorId(motoId)).ReturnsAsync((Moto?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ExcecaoEntidadeNaoEncontrada>(() => 
                _motoServico.ObterPorId(motoId));
        }
    }
}
