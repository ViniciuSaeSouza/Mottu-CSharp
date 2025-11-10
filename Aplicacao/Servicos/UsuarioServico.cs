using Aplicacao.DTOs.Usuario;
using Aplicacao.Validacoes;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;

namespace Aplicacao.Servicos;

public class UsuarioServico
{
    private readonly IRepositorioUsuario _repositorio;
    private readonly IRepositorio<Patio> _patioRepositorio;

    public UsuarioServico(IRepositorioUsuario repositorio, IRepositorio<Patio> patioRepositorio)
    {
        _repositorio = repositorio;
        _patioRepositorio = patioRepositorio;
    }

    public async Task<IEnumerable<UsuarioLeituraDto>> ObterTodos()
    {
        var usuarios = await _repositorio.ObterTodos();

        return usuarios.Count > 0
            ? usuarios.Select(u => new UsuarioLeituraDto(u.Id, u.Nome, u.Email, u.Senha, u.Patio.Nome, u.IdPatio)).ToList()
            : Enumerable.Empty<UsuarioLeituraDto>();
    }

    public async Task<UsuarioLeituraDto> ObterPorId(int id)
    {
        var usuario = await _repositorio.ObterPorId(id);
        ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", id);

        return new UsuarioLeituraDto(usuario!.Id, usuario.Nome, usuario.Email, usuario.Senha, usuario.Patio.Nome, usuario.IdPatio);
    }

    public async Task<UsuarioLeituraDto> ObterPorEmail(string email)
    {
        try
        {
            var usuario = await _repositorio.ObterPorEmail(email);
            ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", email);

            return new UsuarioLeituraDto(usuario!.Id, usuario.Nome, usuario.Email, usuario.Senha, usuario.Patio.Nome, usuario.IdPatio);
        }
        catch (Exception ex)
        {
            throw new ExcecaoDominio("Erro ao obter usuário por email.", nameof(email));
        }
    }

    public async Task<UsuarioLeituraDto> Criar(UsuarioCriarDto dto)
    {
        ValidacaoEntidade.LancarSeNulo(dto, "Usuário", dto);

        var patio = await _patioRepositorio.ObterPorId(dto.IdPatio);
        ValidacaoEntidade.LancarSeNulo(patio, "Pátio", dto.IdPatio);

        var usuario = new Usuario(dto.Nome, dto.Email, dto.Senha, dto.IdPatio);
        usuario.Patio = patio!;

        await _repositorio.Adicionar(usuario);

        return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email,usuario.Senha, usuario.Patio.Nome, usuario.IdPatio);
    }

    public async Task<UsuarioLeituraDto> Atualizar(int id, UsuarioAtualizarDto dto)
    {
        var usuario = await _repositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", id);

        ValidacaoEntidade.AlterarValor(dto.Nome, usuario.AlterarNome);
        ValidacaoEntidade.AlterarValor(dto.Email, usuario.AlterarEmail);
        ValidacaoEntidade.AlterarValor(dto.Senha, usuario.AlterarSenha);

        await _repositorio.Atualizar(usuario!);

        return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email,usuario.Senha, usuario.Patio.Nome, usuario.IdPatio);
    }

    public async Task Remover(int id)
    {
        var usuario = await _repositorio.ObterPorId(id);

        ValidacaoEntidade.LancarSeNulo(usuario, "Usuário", id);

        await _repositorio.Remover(usuario!);
    }

    public async Task<UsuarioLeituraDto> AutenticarLogin(UsuarioLoginDto usuarioLoginDto)
    {
        var email = usuarioLoginDto.email;
        var senha = usuarioLoginDto.senha;
        
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ExcecaoDominio("Email não pode ser vazio.", nameof(email));
        }
        if (string.IsNullOrWhiteSpace(senha))
        {
            throw new ExcecaoDominio("Senha não pode ser vazia.", nameof(senha));
        }
        
        var usuario = await _repositorio.ObterPorEmail(email);

        if (usuario == null)
        {
            throw new ExcecaoEntidadeNaoEncontrada("Usuário não encontrado.", email);
        }
        
        if (usuario.Senha != senha)
        {
            throw new ExcecaoDominio("Senha inválida.", nameof(senha));
        }
        
        var usuarioDto = new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Senha, usuario.Patio.Nome, usuario.IdPatio);
        
        return usuarioDto;
    }
}