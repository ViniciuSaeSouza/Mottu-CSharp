using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace API.Aplicacao.Repositorios
{
    public class MotoRepositorio : IRepositorio<Moto>
    {

        private readonly AppDbContext _context;

        public MotoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Moto> Adicionar(Moto moto)
        {
            await _context.Motos.AddAsync(moto);
            await _context.SaveChangesAsync();

            return moto;
        }

        public async Task<Moto> Atualizar(Moto moto)
        {
            _context.Motos.Update(moto);
            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return moto;
        }

        public async Task<Moto> ObterPorId(int id) =>
            await _context.Motos.Include(m => m.Filial).FirstOrDefaultAsync(m => m.Id == id);



        public async Task<List<Moto>> ObterTodos() =>
            await _context.Motos.Include(m => m.Filial).OrderBy(m => m.Id).ToListAsync();


        public async Task<bool> Remover(Moto moto)
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}