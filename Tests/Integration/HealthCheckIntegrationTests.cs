using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using Xunit;

namespace Tests.Integration
{
    public class HealthCheckIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public HealthCheckIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task HealthCheck_Geral_DeveRetornarHealthy()
        {
            // Act
            var response = await _client.GetAsync("/health");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Healthy");
        }

        [Fact]
        public async Task HealthCheck_Live_DeveRetornarHealthy()
        {
            // Act
            var response = await _client.GetAsync("/health/live");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Healthy");
        }

        [Fact]
        public async Task HealthCheck_Ready_DeveRetornarStatusDependencias()
        {
            // Act
            var response = await _client.GetAsync("/health/ready");

            // Assert
            // O status pode ser Healthy ou Unhealthy dependendo da disponibilidade do Oracle
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("oracle-db");
            content.Should().Contain("carrapato_repositorio");
            content.Should().Contain("api-health");
        }

        [Fact]
        public async Task HealthCheck_Endpoints_DevemRetornarJsonFormatado()
        {
            // Arrange
            var endpoints = new[] { "/health", "/health/live", "/health/ready" };

            foreach (var endpoint in endpoints)
            {
                // Act
                var response = await _client.GetAsync(endpoint);

                // Assert
                response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
                
                var content = await response.Content.ReadAsStringAsync();
                content.Should().StartWith("{");
                content.Should().EndWith("}");
            }
        }

        [Fact]
        public async Task HealthCheck_Ready_DeveConterInformacoesDetalhadas()
        {
            // Act
            var response = await _client.GetAsync("/health/ready");

            // Assert
            var content = await response.Content.ReadAsStringAsync();
            
            // Verificar se contém informações de cada health check
            content.Should().Contain("status");
            content.Should().Contain("totalDuration");
            content.Should().Contain("entries");
        }
    }
}
