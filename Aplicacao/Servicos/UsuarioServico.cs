using Aplicacao.DTOs.Patio;
using Aplicacao.DTOs.Usuario;
using Aplicacao.Validacoes;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Repositorios;

namespace Aplicacao.Servicos;

//TODO: Adicionar validações específicas do usuário (e.g., email único, formato de email, tamanho da senha, etc.)
public class UsuarioServico
{
    private readonly IRepositorio<Usuario> _repositorio;
    private readonly IRepositorio<Patio> _patioRepositorio;

    public UsuarioServico(IRepositorio<Usuario> repositorio, IRepositorio<Patio> patioRepositorio)
    {
        _repositorio = repositorio;
        _patioRepositorio = patioRepositorio;
    }

    public async Task<IEnumerable<UsuarioLeituraDto>> ObterTodos()
    {
        var usuarios = await _repositorio.ObterTodos();

        return usuarios.Count > 0
            ? usuarios.Select(u => new UsuarioLeituraDto(u.Id, u.Nome, u.Email, u.Patio.Nome, u.IdPatio)).ToList()
            : Enumerable.Empty<UsuarioLeituraDto>();
    }

    public async Task<UsuarioLeituraDto> ObterPorId(int id)
    {
        var usuario = await _repositorio.ObterPorId(id);
        ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", id);

        return new UsuarioLeituraDto(usuario!.Id, usuario.Nome, usuario.Email, usuario.Patio.Nome, usuario.IdPatio);
    }

    public async Task<UsuarioLeituraDto> Criar(UsuarioCriarDto dto)
    {
        ValidacaoEntidade.LancarSeNulo(dto, "Usuário", dto);

        var patio = await _patioRepositorio.ObterPorId(dto.IdPatio);
        ValidacaoEntidade.LancarSeNulo(patio, "Pátio", dto.IdPatio);

        var usuario = new Usuario(dto.Nome, dto.Email, dto.Senha, dto.IdPatio);
        usuario.Patio = patio!;

        await _repositorio.Adicionar(usuario);

        return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Patio.Nome, usuario.IdPatio);
    }

    public async Task<UsuarioLeituraDto> Atualizar(int id, UsuarioAtualizarDto dto)
    {
        var usuario = await _repositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", id);

        ValidacaoEntidade.AlterarValor(dto.Nome, usuario.AlterarNome);
        ValidacaoEntidade.AlterarValor(dto.Email, usuario.AlterarEmail);
        ValidacaoEntidade.AlterarValor(dto.Senha, usuario.AlterarSenha);

        await _repositorio.Atualizar(usuario!);

        return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Patio.Nome, usuario.IdPatio);
    }

    public async Task Remover(int id)
    {
        var usuario = await _repositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", id);

        await _repositorio.Remover(usuario!);
    }
}