using Dominio.Excecao;
using Dominio.Interfaces.Mottu;
using Dominio.Persistencia.Mottu;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios.Mottu;

public class MotoMottuRepositorio : IMottuRepositorio
{
    private readonly AppDbContext _context;

    public MotoMottuRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MotoMottu>> ListarAssincrono()
    {
        try
        {
            return await _context.MotosMottu.ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao listar motos da base da mottu",
                nameof(MotoMottu), ex);
        }
    }

    public async Task<MotoMottu?> ObterPorIdAssincrono(int id)
    {
        try
        {
            return await _context.MotosMottu.FindAsync(id);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto por id na base da mottu",
                nameof(MotoMottu), ex);
        }
    }

    public Task<MotoMottu?> ObterPorPlacaAssincrono(string placa)
    {
        try
        {
            return _context.MotosMottu.FirstOrDefaultAsync(m => m.Placa == placa);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto por placa na base da mottu",
                nameof(MotoMottu), ex);
        }
    }

    public async Task<MotoMottu?> ObterPorChassiAssincrono(string chassi)
    {
        try
        {
            var moto = await _context.MotosMottu.FirstOrDefaultAsync(m => m.Chassi == chassi);
            return moto;
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter moto por chassi na base da mottu",
                nameof(MotoMottu), ex);
        }
    }
}