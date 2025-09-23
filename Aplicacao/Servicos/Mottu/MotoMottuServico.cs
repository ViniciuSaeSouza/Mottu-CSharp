using Aplicacao.DTOs.Moto.Mottu;
using Dominio.Interfaces.Mottu;
using Dominio.Persistencia.Mottu;

namespace Aplicacao.Servicos.Mottu;

public class MotoMottuServico
{
    private readonly IMottuRepositorio _repositorio;

    public MotoMottuServico(IMottuRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<MotoMottuDto>> ObterTodosAssincrono()
    {
        var motos = await _repositorio.ListarAssincrono();
        var motosDto = motos.Select(moto => new MotoMottuDto(moto.Id, moto.Placa, moto.Chassi, moto.Modelo));
        return motosDto;
    }


    public async Task<MotoMottuDto?> ObterPorIdAssincrono(int id)
    {
        var moto = await _repositorio.ObterPorIdAssincrono(id);
        var motoDto = moto is not null
            ? new MotoMottuDto(moto.Id, moto.Placa, moto.Chassi, moto.Modelo)
            : null;
        return motoDto;
    }

}