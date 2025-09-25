using Dominio.Interfaces;
using Dominio.Persistencia;

namespace Infraestrutura.Repositorios;

// TODO: Implementar os métodos do repositório 
public class CarrapatoRepositorio : IRepositorio<Carrapato>
{
    public Task<List<Carrapato>> ObterTodos()
    {
        throw new NotImplementedException();
    }

    public Task<(List<Carrapato> Items, int TotalItems)> ObterTodosPaginado(int pagina, int totalPaginas)
    {
        throw new NotImplementedException();
    }

    public Task<Carrapato?> ObterPorId(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Carrapato> Adicionar(Carrapato entity)
    {
        throw new NotImplementedException();
    }

    public Task<Carrapato> Atualizar(Carrapato entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Remover(Carrapato entity)
    {
        throw new NotImplementedException();
    }
}