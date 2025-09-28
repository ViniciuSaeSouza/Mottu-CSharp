using Dominio.Persistencia;

namespace Dominio.Interfaces;

public interface IRepositorioCarrapato : IRepositorio<Carrapato>
{
    Task<Carrapato?> ObterPrimeiroCarrapatoDisponivel();
}