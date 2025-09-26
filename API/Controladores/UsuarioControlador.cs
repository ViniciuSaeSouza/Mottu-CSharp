using Aplicacao.DTOs.Usuario;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[Route("api/usuarios")]
[Tags("Usuários")]
public class UsuarioControlador : ControllerBase
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
    public async Task<ActionResult<List<UsuarioLeituraDto>>> ListarUsuarios()
    {
        try
        {
            var usuarios = await _servico.ObterTodos();
            return Ok(usuarios);
        }
        catch (ExcecaoBancoDados ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        catch (ExcecaoDominio ex)
        {
         return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Retorna um usuário específico com base no ID fornecido.
    /// </summary>
    /// <param name="id">O ID do usuário a ser recuperado.</param>
    /// <returns>
    /// Retorna 200 OK com os detalhes do usuário se encontrado.
    /// Retorna 404 Not Found se o usuário com o ID fornecido não for encontrado.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<UsuarioLeituraDto>> ObterUsuarioPorId(int id)
    {
        try
        {
            var usuario = await _servico.ObterPorId(id);
            return Ok(usuario);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
           return StatusCode(StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ExcecaoBancoDados ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (ExcecaoDominio ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Cria um novo usuário com os dados fornecidos.
    /// </summary>
    /// <param name="dto">Os dados do usuário a serem criados.</param>
    /// <returns>
    /// Retorna 201 Created com os detalhes do usuário criado.
    /// Retorna 400 Bad Request se os dados fornecidos forem inválidos.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioLeituraDto>> CriarUsuario([FromBody] UsuarioCriarDto dto)
    {
        try
        {
            var usuario = await _servico.Criar(dto);
            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuario.IdUsuario }, usuario);
        }
        catch (ExcecaoBancoDados ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        catch (ExcecaoDominio ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    /// <summary>
    /// Atualiza um usuário existente com os dados fornecidos.
    /// </summary>
    /// <param name="id">O ID do usuário a ser atualizado.</param>
    /// <param name="dto">Os dados do usuário a serem atualizados.</param>
    /// <returns>
    /// Retorna 200 OK com os detalhes do usuário atualizado.
    /// Retorna 404 Not Found se o usuário com o ID fornecido não for encontrado.
    /// Retorna 400 Bad Request se os dados fornecidos forem inválidos.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<UsuarioLeituraDto>> AtualizarUsuario(int id, [FromBody] UsuarioAtualizarDto dto)
    {
        try
        {
            var usuario = await _servico.Atualizar(id, dto);
            return Ok(usuario);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            return StatusCode(StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ExcecaoBancoDados ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        catch (ExcecaoDominio ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    /// <summary>
    /// Remove um usuário existente com base no ID fornecido.
    /// </summary>
    /// <param name="id">O ID do usuário a ser removido.</param>
    /// <returns>
    /// Retorna 204 No Content se o usuário for removido com sucesso.
    /// Retorna 404 Not Found se o usuário com o ID fornecido não for encontrado.
    /// Retorna 400 Bad Request se os dados fornecidos forem inválidos.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> DeletarUsuario(int id)
    {
        try
        {
            await _servico.Remover(id);
            return NoContent();
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            return StatusCode(StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ExcecaoBancoDados ex)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        catch (ExcecaoDominio ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Autentica um usuário com email e senha.
    /// </summary>
    /// <param name="usuarioLoginDto">Os dados de login do usuário (email e senha).</param>
    /// <returns>
    /// Retorna 200 OK com os detalhes do usuário autenticado.
    /// Retorna 401 Unauthorized se a autenticação falhar.
    /// Retorna 404 Not Found se o usuário com o email fornecido não for encontrado
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
    {
        try
        {
            var resultado = await _servico.AutenticarLogin(usuarioLoginDto);
            return Ok(resultado);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine(ex.StackTrace);
            return StatusCode(StatusCodes.Status404NotFound, ex.Message);
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine(ex.StackTrace);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine(ex.StackTrace);
            return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
        
    }
}