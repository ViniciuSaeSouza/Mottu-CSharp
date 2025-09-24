using Aplicacao.DTOs.Usuario;
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
            ? usuarios.Select(u => new UsuarioLeituraDto(u.Id, u.Nome, u.Email, u.Patio.Nome)).ToList()
            : Enumerable.Empty<UsuarioLeituraDto>();
    }
}