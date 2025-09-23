using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

public class FilialRepositorio : IRepositorio<Patio>
{
    private readonly AppDbContext _context;

    public FilialRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patio> Adicionar(Patio patio)
    {
        try
        {
            await _context.Filiais.AddAsync(patio);
            await _context.SaveChangesAsync();

            return patio;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao adicionar filial no banco de dados", nameof(patio), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração no banco de dados", nameof(patio), innerException: ex);
        }
    }

    public async Task<Patio> Atualizar(Patio patio)
    {
        try
        {
            _context.Filiais.Update(patio);
            _context.Entry(patio).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return patio;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao atualizar filial no banco de dados", nameof(patio), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração de filial no banco de dados", nameof(patio), innerException: ex);
        }
    }

    public async Task<Patio> ObterPorId(int id) =>
        await _context.Filiais.Include(f => f.Motos).FirstOrDefaultAsync(f => f.Id == id);

    public async Task<List<Patio>> ObterTodos() =>
        await _context.Filiais.OrderBy(f => f.Id).ToListAsync();

    public async Task<bool> Remover(Patio patio)
    {
        try
        {
            _context.Filiais.Remove(patio);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao remover filial do banco de dados", nameof(patio), ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar remoção de filial no banco de dados", nameof(patio), innerException: ex);
        }
    }
}