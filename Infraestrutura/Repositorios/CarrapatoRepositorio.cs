using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Modelo;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

// TODO: Implementar os métodos do repositório 
public class CarrapatoRepositorio : IRepositorioCarrapato
{
    private readonly AppDbContext _contexto;

    public CarrapatoRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Carrapato>> ObterTodos()
    {
        try
        {
            return await _contexto.Carrapatos.ToListAsync();
        }
        catch (OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter carrapatos do banco de dados",
                nameof(Carrapato), exception);
        }
    }

    public async Task<IResultadoPaginado<Carrapato>> ObterTodosPaginado(int pagina, int tamanho)
    {
        try
        {
            var consulta = _contexto.Carrapatos.AsNoTracking();

            var totalRegistros = await consulta.CountAsync();
            var carrapatos = await consulta
                .Skip((pagina - 1) * tamanho)
                .Take(tamanho)
                .ToListAsync();

            var carrapatosPaginados = new ResultadoPaginado<Carrapato>
            {
                ContagemTotal = totalRegistros,
                Pagina = pagina,
                Items = carrapatos,
                TamanhoPagina = tamanho
            };

            return carrapatosPaginados;
        }
        catch (OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter carrapatos do banco de dados",
                nameof(Carrapato), exception);
        }
    }

    public async Task<Carrapato?> ObterPrimeiroCarrapatoDisponivel()
    {
        try
        {
            var carrapatos = await _contexto.Carrapatos.ToListAsync();

            var primeiroCarrapatoDisponivel = carrapatos
                .FirstOrDefault(c => c.StatusDeUso == StatusDeUsoEnum.Disponivel);
            return primeiroCarrapatoDisponivel;

        }
        catch (ArgumentNullException ex)
        {
            throw new ExcecaoBancoDados("Falha ao obter carrapato do banco de dados",
                nameof(Carrapato), ex);
        }
        catch (OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter carrapato do banco de dados",
                nameof(Carrapato), exception);
        }
        
    }
    
    public async Task<Carrapato?> ObterPorId(int id)
    {
        try
        {
            return await _contexto.Carrapatos.FindAsync(id);
        }
        catch (OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter carrapato do banco de dados",
                nameof(Carrapato), exception);
        }
    }

    public async Task<Carrapato> Adicionar(Carrapato carrapatoDto)
    {
        try
        {
            var carrapatoCriado = _contexto.Carrapatos.Add(carrapatoDto);
            await _contexto.SaveChangesAsync();
            return carrapatoCriado.Entity;
        }
        catch(OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao adicionar carrapato no banco de dados",
                nameof(Carrapato), exception);
        }
        catch(DbUpdateException exception)
        {
            throw new ExcecaoBancoDados("Falha ao adicionar carrapato no banco de dados",
                nameof(Carrapato), exception);
        }
    }

    public async Task<Carrapato> Atualizar(Carrapato carrapato)
    {
        try
        {
            _contexto.Carrapatos.Update(carrapato);
            await _contexto.SaveChangesAsync();
            return carrapato;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new ExcecaoBancoDados("Falha de concorrência ao atualizar carrapato no banco de dados",
                nameof(Carrapato), exception);
        }
        catch (OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao atualizar carrapato no banco de dados",
                nameof(Carrapato), exception);
        }
        catch (DbUpdateException exception)
        {
            throw new ExcecaoBancoDados("Falha ao atualizar carrapato no banco de dados",
                nameof(Carrapato), exception);
        }
    }

    public async Task<bool> Remover(Carrapato carrapato)
    {
        try
        {
            _contexto.Carrapatos.Remove(carrapato);
            await _contexto.SaveChangesAsync();
            return true;
        }
        catch (OperationCanceledException exception)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao remover carrapato do banco de dados",
                nameof(Carrapato), exception);
        }
        catch (DbUpdateException exception)
        {
            throw new ExcecaoBancoDados("Falha ao remover carrapato do banco de dados",
                nameof(Carrapato), exception);
        }
    }
}