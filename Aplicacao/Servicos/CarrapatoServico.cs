using Aplicacao.DTOs.Carrapato;
using Aplicacao.Validacoes;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;

namespace Aplicacao.Servicos;

public class CarrapatoServico
{
    private readonly IRepositorioCarrapato _repositorio;

    public CarrapatoServico(IRepositorioCarrapato repositorio)
    {
        _repositorio = repositorio;
    }

    private CarrapatoLeituraDto MapearParaDto(Carrapato carrapato)
    {
        return new CarrapatoLeituraDto(
            carrapato.Id,
            carrapato.CodigoSerial,
            carrapato.StatusBateria.ToString(),
            carrapato.StatusDeUso.ToString(),
            carrapato.IdPatio
        );
    }

    public async Task<IEnumerable<CarrapatoLeituraDto>> ObterTodos()
    {
        var carrapatos = await _repositorio.ObterTodos();
        return carrapatos.Select(MapearParaDto).ToList();
    }

    public async Task<CarrapatoLeituraDto> ObterPorId(int id)
    {
        var carrapato = await _repositorio.ObterPorId(id);
        ValidacaoEntidade.LancarSeNulo(carrapato, "Carrapato", id);
        return MapearParaDto(carrapato!);
    }

    public async Task<CarrapatoLeituraDto> Criar(CarrapatoCriarDto dto)
    {
        var carrapato = new Carrapato(dto.CodigoSerial, dto.IdPatio);
        var criado = await _repositorio.Adicionar(carrapato);
        return MapearParaDto(criado);
    }

    public async Task<CarrapatoLeituraDto> Atualizar(int id, CarrapatoCriarDto dto)
    {
        var carrapato = await _repositorio.ObterPorId(id);
        ValidacaoEntidade.LancarSeNulo(carrapato, "Carrapato", id);
        carrapato!.CodigoSerial = dto.CodigoSerial;
        carrapato.IdPatio = dto.IdPatio;
        var atualizado = await _repositorio.Atualizar(carrapato);
        return MapearParaDto(atualizado);
    }

    public async Task<bool> Remover(int id)
    {
        var carrapato = await _repositorio.ObterPorId(id);
        ValidacaoEntidade.LancarSeNulo(carrapato, "Carrapato", id);
        return await _repositorio.Remover(carrapato!);
    }
}