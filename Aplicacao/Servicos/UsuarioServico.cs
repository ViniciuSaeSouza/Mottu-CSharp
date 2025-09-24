using Aplicacao.DTOs.Usuario;
using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;

namespace Aplicacao.Servicos;

public class UsuarioServico
{
    private readonly IRepositorio<Usuario> _repositorio;

    public UsuarioServico(IRepositorio<Usuario> repositorio)
    {
        _repositorio = repositorio;
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
            if (usuario == null)
            {
                throw new ExcecaoEntidadeNaoEncontrada("Usuário não encontrado", id);
            }

            return new UsuarioLeituraDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Patio.Nome, usuario.IdPatio); 
    }
}