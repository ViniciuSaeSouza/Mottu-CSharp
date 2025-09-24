using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[Route("api/usuarios")]
[Tags("Usuários")]
public class UsuarioControlador
{
    private readonly UsuarioServico _servico;

    public UsuarioControlador(UsuarioServico servico)
    {
        _servico = servico;
    }

    /// <summary>
    /// Retorna a lista de usuários cadastrados no sistema.
    /// </summary>
    /// <returns>
    /// Retorna 200 OK com a lista de usuários cadastrados.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ListarUsuarios()
    {
        try
        {
            var usuarios = await _servico.ObterTodos();
            return new OkObjectResult(usuarios);
        }
        catch (ExcecaoBancoDados ex)
        {
            return new ObjectResult("Serviço de banco de dados indisponível")
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable
            };
        }
        catch (ExcecaoDominio ex)
        {
            return new ObjectResult("Falha ao processar requisição")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
        catch (Exception ex)
        {
            return new ObjectResult("Erro interno do servidor")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}