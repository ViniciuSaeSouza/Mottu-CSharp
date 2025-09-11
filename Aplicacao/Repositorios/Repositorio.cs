using Dominio.Interfaces;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace API.Aplicacao.Repositorios
{
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

        public async Task<T> ObterPorId(int id) => await _dbSet.FirstOrDefaultAsync(e => e.Equals(id));

        public Task<List<T>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remover(T entity)
        {
            throw new NotImplementedException();
        }

    }
}