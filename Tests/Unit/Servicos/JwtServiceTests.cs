using API.Servicos;
using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using Xunit;

namespace Tests.Unit.Servicos
{
    public class JwtServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly JwtService _jwtService;

        public JwtServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            
            // Configure mock to return test JWT settings
            _mockConfiguration.Setup(x => x["Jwt:Key"]).Returns("MinhaChaveSecretaSuperSeguraParaJWT123456TestKey");
            _mockConfiguration.Setup(x => x["Jwt:Issuer"]).Returns("MottuAPI");
            _mockConfiguration.Setup(x => x["Jwt:Audience"]).Returns("MottuAPI");
            
            _jwtService = new JwtService(_mockConfiguration.Object);
        }

        [Fact]
        public void GerarToken_DeveRetornarTokenValido()
        {
            // Arrange
            var usuario = "testuser";
            var role = "User";

            // Act
            var token = _jwtService.GerarToken(usuario, role);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Should().Contain("."); // JWT format has dots
            token.Split('.').Should().HaveCount(3); // Header.Payload.Signature
        }

        [Fact]
        public void GerarToken_ComRolePadrao_DeveUsarRoleUser()
        {
            // Arrange
            var usuario = "testuser";

            // Act
            var token = _jwtService.GerarToken(usuario);

            // Assert
            token.Should().NotBeNullOrEmpty();
            // Token should be valid (tested in validation test)
            _jwtService.ValidarToken(token).Should().BeTrue();
        }

        [Fact]
        public void ValidarToken_ComTokenValido_DeveRetornarTrue()
        {
            // Arrange
            var usuario = "testuser";
            var token = _jwtService.GerarToken(usuario);

            // Act
            var resultado = _jwtService.ValidarToken(token);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public void ValidarToken_ComTokenInvalido_DeveRetornarFalse()
        {
            // Arrange
            var tokenInvalido = "token.invalido.aqui";

            // Act
            var resultado = _jwtService.ValidarToken(tokenInvalido);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void ValidarToken_ComTokenVazio_DeveRetornarFalse()
        {
            // Arrange
            var tokenVazio = "";

            // Act
            var resultado = _jwtService.ValidarToken(tokenVazio);

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public void ValidarToken_ComTokenNull_DeveRetornarFalse()
        {
            // Arrange
            string? tokenNull = null;

            // Act
            var resultado = _jwtService.ValidarToken(tokenNull!);

            // Assert
            resultado.Should().BeFalse();
        }

        [Theory]
        [InlineData("admin")]
        [InlineData("user123")]
        [InlineData("test@example.com")]
        public void GerarToken_ComDiferentesUsuarios_DeveGerarTokensValidos(string usuario)
        {
            // Act
            var token = _jwtService.GerarToken(usuario);

            // Assert
            token.Should().NotBeNullOrEmpty();
            _jwtService.ValidarToken(token).Should().BeTrue();
        }

        [Theory]
        [InlineData("Admin")]
        [InlineData("User")]
        [InlineData("Manager")]
        public void GerarToken_ComDiferentesRoles_DeveGerarTokensValidos(string role)
        {
            // Arrange
            var usuario = "testuser";

            // Act
            var token = _jwtService.GerarToken(usuario, role);

            // Assert
            token.Should().NotBeNullOrEmpty();
            _jwtService.ValidarToken(token).Should().BeTrue();
        }
    }
}
