using Aplicacao.DTOs.Patio;
using Aplicacao.DTOs.Moto;
using Aplicacao.DTOs.Usuario;
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

        var motosDto = patios
            .Select(p =>
                p.Motos.Select(m => new MotoLeituraDto(m.Id, m.Placa, m.Modelo.ToString(), p.Nome, m.Chassi, m.Zona)))
            .SelectMany(m => m).ToList();

        var usuariosDto = patios
            .Select(p => p.Usuarios.Select(u => new UsuarioLeituraDto(u.Id, u.Nome, u.Email, u.Patio.Nome, u.IdPatio)))
            .SelectMany(u => u).ToList();

        var patiosDto = patios
            .Select(p => new PatioLeituraDto(p.Id, p.Nome, p.Endereco,
                motosDto.Where(m => m.NomePatio == p.Nome).ToList(),
                usuariosDto.Where(u => u.NomePatio == p.Nome).ToList()))
            .ToList();

        return patiosDto;
    }

    public async Task<PatioLeituraDto> ObterPorId(int id)
    {
        var patio = await _patioRepositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(patio, "Patio", id);

        return new PatioLeituraDto(patio.Id, patio.Nome, patio.Endereco,
            patio.Motos.Select(m =>
                    new MotoLeituraDto(m.Id, m.Placa, m.Modelo.ToString().ToUpper(), patio.Nome, m.Chassi, m.Zona))
                .ToList(),
            patio.Usuarios.Select(u =>
                new UsuarioLeituraDto(u.Id, u.Nome, u.Email, patio.Nome, u.IdPatio)).ToList()
        );
    }

    public async Task<PatioLeituraDto> Criar(PatioCriarDto dto)
    {
        // entrada inválida do user
        if (dto == null)
            throw new ExcecaoDominio("Patio não pode ser nulo.", nameof(dto));

        var patio = new Patio(dto.Nome, dto.Endereco);

        await _patioRepositorio.Adicionar(patio);

        return new PatioLeituraDto(patio.Id, patio.Nome, patio.Endereco, new List<MotoLeituraDto>(),
            new List<UsuarioLeituraDto>());
    }

    public async Task<PatioLeituraDto> Atualizar(int id, PatioAtualizarDto dto)
    {
        var patio = await _patioRepositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(patio, "Patio", id);

        ValidacaoEntidade.AlterarValor(dto.Nome, patio.AlterarNome);
        ValidacaoEntidade.AlterarValor(dto.Endereco, patio.AlterarEndereco);

        await _patioRepositorio.Atualizar(patio);

        return new PatioLeituraDto(patio.Id, patio.Nome, patio.Endereco,
            patio.Motos.Select(m =>
                    new MotoLeituraDto(m.Id, m.Placa, m.Modelo.ToString(), patio.Nome, m.Chassi, m.Zona))
                .ToList(),
            patio.Usuarios.Select(u =>
                new UsuarioLeituraDto(u.Id, u.Nome, u.Email, patio.Nome, u.IdPatio)).ToList()
        );
    }

    public async Task Remover(int id)
    {
        var patio = await _patioRepositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(patio, "Patio", id);

        await _patioRepositorio.Remover(patio);
    }
}