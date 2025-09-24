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
            throw new ExcecaoBancoDados("Falha, operação cancelada ao adicionar moto no banco de dados", nameof(moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração no banco de dados", nameof(moto), innerException: ex);
        }
    }

    public async Task<Moto> Atualizar(Moto moto)
    {
        try
        {
            _context.Motos.Update(moto);
            await _context.SaveChangesAsync();
            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao atualizar moto no banco de dados", nameof(moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração de moto no banco de dados", nameof(moto), innerException: ex);
        }
    }

    public async Task<Moto?> ObterPorId(int id)
    {
        try
        {
            return await _context.Motos.Include(m => m.Patio).FirstOrDefaultAsync(m => m.Id == id);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto do banco de dados", nameof(Moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao obter moto do banco de dados", nameof(Moto), innerException: ex);
        }
    }

    public async Task<List<Moto>> ObterTodos()
    {
        try
        {
            return await _context.Motos.Include(m => m.Patio).OrderBy(m => m.Id).ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter motos do banco de dados", nameof(Moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao obter motos do banco de dados", nameof(Moto), innerException: ex);
        }
        
    }

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
            throw new ExcecaoBancoDados("Falha, operação cancelada ao remover moto no banco de dados", nameof(moto), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração de moto no banco de dados", nameof(moto), innerException: ex);
        }
    }
}