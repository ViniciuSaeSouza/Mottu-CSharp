using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

public class PatioRepositorio : IRepositorio<Patio>
{
    private readonly AppDbContext _context;

    public PatioRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patio> Adicionar(Patio patio)
    {
        try
        {
            await _context.Patios.AddAsync(patio);
            await _context.SaveChangesAsync();

            return patio;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao adicionar patio no banco de dados", nameof(patio),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração no banco de dados", nameof(patio),
                innerException: ex);
        }
    }

    public async Task<Patio> Atualizar(Patio patio)
    {
        try
        {
            _context.Patios.Update(patio);
            _context.Entry(patio).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return patio;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao atualizar patio no banco de dados", nameof(patio),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração de patio no banco de dados", nameof(patio),
                innerException: ex);
        }
    }

    public Task<(List<Patio> Items, int TotalItems)> ObterTodosPaginado(int pagina, int totalPaginas)
    {
        throw new NotImplementedException();
    }

    public async Task<Patio?> ObterPorId(int id)
    {
        try
        {
            return await _context.Patios.Include(p => p.Motos).Include(p => p.Usuarios)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter patio do banco de dados", nameof(Patio),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao obter patio do banco de dados", nameof(Patio),
                innerException: ex);
        }
    }

    public async Task<List<Patio>> ObterTodos()
    {
        try
        {
            return await _context.Patios.OrderBy(f => f.Id).ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter patios do banco de dados", nameof(Patio),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao obter patios do banco de dados", nameof(Patio),
                innerException: ex);
        }
    }

    public async Task<bool> Remover(Patio patio)
    {
        try
        {
            _context.Patios.Remove(patio);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao remover patio do banco de dados", nameof(patio),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar remoção de patio no banco de dados", nameof(patio),
                innerException: ex);
        }
    }
}