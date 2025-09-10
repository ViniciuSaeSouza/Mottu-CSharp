using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace API.Application;

public class FilialRepositorio : IRepositorio<Filial>
{

    private readonly AppDbContext _context;

    public FilialRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Filial> Adicionar(Filial filial)
    {
        await _context.Filiais.AddAsync(filial);
        await _context.SaveChangesAsync();

        return filial;
    }

    public async Task<Filial> Atualizar(Filial filial)
    {
        _context.Filiais.Update(filial);
        _context.Entry(filial).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return filial;
    }

    public async Task<Filial> ObterPorId(int id) =>
        await _context.Filiais.Include(f => f.Motos).FirstOrDefaultAsync(f => f.Id == id);



    public async Task<List<Filial>> ObterTodos() =>
        await _context.Filiais.OrderBy(f => f.Id).ToListAsync();


    public async Task<bool> Remover(Filial filial)
    {
        _context.Filiais.Remove(filial);
        await _context.SaveChangesAsync();
        return true;
    }
}