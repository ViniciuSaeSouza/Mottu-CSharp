using Dominio.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.Saude;

public sealed class CarrapatoHealthCheck : IHealthCheck
{
    private readonly IRepositorioCarrapato _repositorioCarrapato;

    public CarrapatoHealthCheck(IRepositorioCarrapato repositorioCarrapato)
    {
        _repositorioCarrapato = repositorioCarrapato;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            var resultado = await _repositorioCarrapato.ObterTodosPaginado(1, 1);
            
            return HealthCheckResult.Healthy("Repositorio Ok");
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy("Erro ao acessar o repositorio", e);
        }
    }
}