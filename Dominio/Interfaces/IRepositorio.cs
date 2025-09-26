using Dominio.Persistencia;

namespace Dominio.Interfaces;

public interface IRepositorio<T> where T : class
{
    Task<List<T>> ObterTodos();
    
    Task<IResultadoPaginado<T>> ObterTodosPaginado(int pagina, int totalPaginas);

    Task<T?> ObterPorId(int id);

    Task<T> Adicionar(T entity);

    Task<T> Atualizar(T entity);

    Task<bool> Remover(T entity);
}
