using Dominio.Persistencia.Mottu;

namespace Dominio.Interfaces.Mottu;

public interface IMottuRepositorio
{
    public Task<List<MotoMottu>> ListarAssincrono();
    
    public  Task<MotoMottu?> ObterPorIdAssincrono(int id);
}