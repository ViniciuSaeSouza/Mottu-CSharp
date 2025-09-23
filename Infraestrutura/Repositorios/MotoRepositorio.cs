using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

public class MotoRepositorio : IRepositorio<Moto>
{
    private readonly AppDbContext _context;

    public MotoRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Moto> Adicionar(Moto moto)
    {
        try
        {
            await _context.Motos.AddAsync(moto);
            await _context.SaveChangesAsync();

            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, opera��o cancelada ao adicionar moto no banco de dados", nameof(moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar altera��o no banco de dados", nameof(moto), innerException: ex);
        }
    }

    public async Task<Moto> Atualizar(Moto moto)
    {
        try
        {
            _context.Motos.Update(moto);
            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, opera��o cancelada ao atualizar moto no banco de dados", nameof(moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar altera��o de moto no banco de dados", nameof(moto), innerException: ex);
        }
    }

    public async Task<Moto> ObterPorId(int id) =>
        await _context.Motos.Include(m => m.Patio).FirstOrDefaultAsync(m => m.Id == id);

    public async Task<List<Moto>> ObterTodos() =>
        await _context.Motos.Include(m => m.Patio).OrderBy(m => m.Id).ToListAsync();

    public async Task<bool> Remover(Moto moto)
    {
        try
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, opera��o cancelada ao remover moto no banco de dados", nameof(moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar altera��o de moto no banco de dados", nameof(moto), innerException: ex);
        }
    }
}