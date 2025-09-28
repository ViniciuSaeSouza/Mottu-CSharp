using Dominio;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Modelo;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

public class MotoRepositorio : IMotoRepositorio
{
    private readonly AppDbContext _contexto;

    public MotoRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Moto> Adicionar(Moto moto)
    {
        try
        {
            await _contexto.Motos.AddAsync(moto);
            await _contexto.SaveChangesAsync();
            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao adicionar moto no banco de dados", nameof(moto),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração no banco de dados", nameof(moto),
                innerException: ex);
        }
    }

    public async Task<Moto> Atualizar(Moto moto)
    {
        try
        {
            _contexto.Motos.Update(moto);
            await _contexto.SaveChangesAsync();
            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao atualizar moto no banco de dados", nameof(moto),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração de moto no banco de dados", nameof(moto),
                innerException: ex);
        }
    }

    public async Task<Moto?> ObterPorId(int id)
    {
        try
        {
            return await _contexto.Motos.Include(m => m.Patio).Include(m => m.Carrapato).FirstOrDefaultAsync(m => m.Id == id);
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
            return await _contexto.Motos.Include(m => m.Patio).OrderBy(m => m.Id).ToListAsync();
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

    public async Task<IResultadoPaginado<Moto>> ObterTodosPaginado(int pagina, int tamanhoPagina)
    {
        
        var consulta = _contexto.Motos
            .Include(m => m.Patio)
            .OrderBy(m => m.Id)
            .AsNoTracking();

        var totalMotos = await consulta.CountAsync();
        var motos = await consulta
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        var motosPaginadas = new ResultadoPaginado<Moto>
        {
            ContagemTotal = totalMotos,
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            Items = motos
        };
        
        return motosPaginadas;
    }
    
    public async Task<Moto?> ObterPorPlaca(string placa)
    {
        try
        {
            return await _contexto.Motos.Include(m => m.Patio).FirstOrDefaultAsync(m => m.Placa == placa);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto do banco de dados", nameof(Moto), ex);
        }
        catch (Exception ex)
        {
            throw new ExcecaoBancoDados($"Falha ao obter moto do banco de dados pela placa {placa}", nameof(Moto), innerException: ex);
        }
    }

    public async Task<bool> Remover(Moto moto)
    {
        try
        {
            _contexto.Motos.Remove(moto);
            await _contexto.SaveChangesAsync();
            return true;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao remover moto no banco de dados", nameof(moto),
                ex);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao salvar alteração de moto no banco de dados", nameof(moto),
                innerException: ex);
        }
    }

    public Task<List<Moto>> ListarAssincrono()
    {
        throw new NotImplementedException();
    }

    public Task<Moto?> ObterPorIdAssincrono(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Moto?> ObterPorPlacaAssincrono(string placa)
    {
        try
        {
            var moto = _contexto.Motos.Include(m => m.Patio).FirstOrDefaultAsync(m => m.Placa == placa);
            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto do banco de dados", nameof(Moto), ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new ExcecaoBancoDados($"Falha ao obter moto do banco de dados pela placa {placa}", nameof(Moto), innerException: ex);
        }
    }

    public Task<Moto?> ObterPorChassiAssincrono(string chassi)
    {
        try
        {
            var moto = _contexto.Motos.Include(m => m.Patio).FirstOrDefaultAsync(m => m.Chassi == chassi);
            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto do banco de dados", nameof(Moto), ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            throw new ExcecaoBancoDados($"Falha ao obter moto do banco de dados pelo chassi {chassi}", nameof(Moto), innerException: ex);
        }
    }
}