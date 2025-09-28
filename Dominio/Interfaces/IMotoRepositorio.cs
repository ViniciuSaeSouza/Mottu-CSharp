using Dominio.Interfaces;
using Dominio.Persistencia;

namespace Dominio.Interfaces;

public interface IMotoRepositorio : IRepositorio<Moto>
{
    public Task<Moto?> ObterPorPlacaAssincrono(string placa);
    
    public Task<Moto?> ObterPorChassiAssincrono(string chassi);
    
}