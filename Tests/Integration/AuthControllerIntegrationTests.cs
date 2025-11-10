using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Tests.Integration
{
    public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_ComCredenciaisValidas_DeveRetornarToken()
        {
            // Arrange
            var loginDto = new
            {
                email = "admin",
                senha = "admin123"
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v2.0/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("token");
            responseContent.Should().Contain("Bearer");
            responseContent.Should().Contain("admin");
        }

        [Fact]
        public async Task Login_ComCredenciaisInvalidas_DeveRetornarUnauthorized()
        {
            // Arrange
            var loginDto = new
            {
                email = "usuario_invalido",
                senha = "senha_invalida"
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v2.0/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("admin", "admin123")]
        [InlineData("mottu", "mottu2024")]
        [InlineData("demo", "demo123")]
        [InlineData("test", "test123")]
        public async Task Login_ComTodosUsuariosValidos_DeveRetornarToken(string usuario, string senha)
        {
            // Arrange
            var loginDto = new
            {
                email = usuario,
                senha = senha
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v2.0/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("token");
            responseContent.Should().Contain(usuario);
        }

        [Fact]
        public async Task ValidarToken_ComTokenValido_DeveRetornarOk()
        {
            // Arrange - Primeiro obter um token válido
            var loginDto = new
            {
                email = "admin",
                senha = "admin123"
            };

            var loginJson = JsonSerializer.Serialize(loginDto);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/v2.0/auth/login", loginContent);
            
            loginResponse.EnsureSuccessStatusCode();
            
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<JsonElement>(loginResponseContent);
            
            // More robust token extraction
            if (!loginResult.TryGetProperty("token", out var tokenProperty))
            {
                throw new InvalidOperationException($"Token not found in login response. Response: {loginResponseContent}");
            }
            
            var token = tokenProperty.GetString();

            // Act
            var tokenJson = JsonSerializer.Serialize(token);
            var tokenContent = new StringContent(tokenJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/v2.0/auth/validar-token", tokenContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("\"valido\":true");
        }

        [Fact]
        public async Task ValidarToken_ComTokenInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var tokenInvalido = "token.invalido.aqui";
            var json = JsonSerializer.Serialize(tokenInvalido);
            using (var content = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                // Act
                var response = await _client.PostAsync("/api/v2.0/auth/validar-token", content);

                // Assert
                response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
                
                var responseContent = await response.Content.ReadAsStringAsync();
                responseContent.Should().Contain("\"valido\":false");
            }
        }
    }
}
