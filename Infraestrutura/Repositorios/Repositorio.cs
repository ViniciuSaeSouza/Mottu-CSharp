using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repositorio(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> Adicionar(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<T> Atualizar(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public Task<IResultadoPaginado<T>> ObterTodosPaginado(int pagina, int totalPaginas)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> ObterPorId(int id) => await _dbSet.FindAsync(id);

    public async Task<List<T>> ObterTodos() => await _dbSet.ToListAsync();

    public async Task<bool> Remover(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}