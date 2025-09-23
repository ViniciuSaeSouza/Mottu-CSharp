using Aplicacao.DTOs.Patio;
using Aplicacao.DTOs.Moto;
using Aplicacao.Validacoes;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;

namespace Aplicacao.Servicos;

public class PatioServico
{
    private readonly IRepositorio<Patio> _patioRepositorio;
    private readonly IRepositorio<Moto> _motoRepositorio;

    public PatioServico(IRepositorio<Patio> patioRepositorio, IRepositorio<Moto> motoRepositorio)
    {
        _patioRepositorio = patioRepositorio;
        _motoRepositorio = motoRepositorio;
    }

    public async Task<IEnumerable<PatioLeituraDto>> ObterTodos()
    {
        var patios = await _patioRepositorio.ObterTodos();

        return patios.Select(p => new PatioLeituraDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Endereco = p.Endereco
        }).ToList();
    }

    public async Task<PatioLeituraDto> ObterPorId(int id)
    {
        var patio = await _patioRepositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(patio, "Patio", id);

        return new PatioLeituraDto
        {
            Id = patio.Id,
            Nome = patio.Nome,
            Endereco = patio.Endereco,
            Motos = patio.Motos.Select(m =>
                    new MotoLeituraDto(m.Id, m.Placa, m.Modelo.ToString().ToUpper(), patio.Nome, m.Chassi, m.Zona))
                .ToList()
        };
    }

    public async Task<PatioLeituraDto> Criar(PatioCriarDto dto)
    {
        // entrada inválida do user
        if (dto == null)
            throw new ExcecaoDominio("Patio não pode ser nulo.", nameof(dto));

        var patio = new Patio(dto.Nome, dto.Endereco);

        await _patioRepositorio.Adicionar(patio);

        return new PatioLeituraDto
        {
            Id = patio.Id,
            Nome = patio.Nome,
            Endereco = patio.Endereco,
            Motos = new List<MotoLeituraDto>() // sempre inicializa vazio
        };
    }

    public async Task<PatioLeituraDto> Atualizar(int id, PatioAtualizarDto dto)
    {
        var patio = await _patioRepositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(patio, "Patio", id);

        ValidacaoEntidade.AlterarValor(dto.Nome, patio.AlterarNome);
        ValidacaoEntidade.AlterarValor(dto.Endereco, patio.AlterarEndereco);

        await _patioRepositorio.Atualizar(patio);

        return new PatioLeituraDto
        {
            Id = patio.Id,
            Nome = patio.Nome,
            Endereco = patio.Endereco,
            Motos = patio.Motos.Select(m =>
                    new MotoLeituraDto(m.Id, m.Placa, m.Modelo.ToString().ToUpper(), patio.Nome, m.Chassi, m.Zona))
                .ToList()
        };
    }

    public async Task Remover(int id)
    {
        var patio = await _patioRepositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(patio, "Patio", id);

        await _patioRepositorio.Remover(patio);
    }
}