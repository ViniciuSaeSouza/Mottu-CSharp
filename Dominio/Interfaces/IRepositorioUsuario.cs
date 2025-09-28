using Dominio.Persistencia;

namespace Dominio.Interfaces;

public interface IRepositorioUsuario : IRepositorio<Usuario>
{
    Task<Usuario?> ObterPorEmail(string email);
}