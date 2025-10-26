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
    private readonly ILogger<UsuarioControlador> _logger;

    public UsuarioControlador(UsuarioServico servico, ILogger<UsuarioControlador> logger)
    {
        _servico = servico;
        _logger = logger;
    }

    /// <summary>
    /// Retorna a lista de usuários cadastrados no sistema.
    /// </summary>
    /// <returns>
    /// Retorna 200 OK com a lista de usuários cadastrados.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<UsuarioLeituraDto>>> ListarUsuarios()
    {
        try
        {
            var usuarios = await _servico.ObterTodos();
            
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return Problem("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Retorna um usuário específico com base no ID fornecido.
    /// </summary>
    /// <param name="id">O ID do usuário a ser recuperado.</param>
    /// <returns>
    /// 200 OK com os detalhes do usuário se encontrado.
    /// 404 Not Found se o usuário com o ID fornecido não for encontrado.
    /// 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioLeituraDto>> ObterUsuarioPorId(int id)
    {
        try
        {
            var usuario = await _servico.ObterPorId(id);
            return Ok(usuario);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return BadRequest("Dados de requisição inválidos");
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return NotFound($"Nenhum usuário encontrado para o id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    /// Cria um novo usuário com os dados fornecidos.
    /// </summary>
    /// <param name="dto">Os dados do usuário a serem criados.</param>
    /// <returns>
    /// 201 Created com os detalhes do usuário criado.
    /// 400 Bad Request se os dados fornecidos forem inválidos.
    /// 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
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
    /// 200 OK com os detalhes do usuário atualizado.
    /// 404 Not Found se o usuário com o ID fornecido não for encontrado.
    /// 400 Bad Request se os dados fornecidos forem inválidos.
    /// 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioLeituraDto>> AtualizarUsuario(int id, [FromBody] UsuarioAtualizarDto dto)
    {
        try
        {
            var usuario = await _servico.Atualizar(id, dto);
            
            return Ok(usuario);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest("Dados da requisição inválidos.");
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return NotFound($"Nenhum usuário encontrado para o id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }
    
    /// <summary>
    /// Remove um usuário existente com base no ID fornecido.
    /// </summary>
    /// <param name="id">O ID do usuário a ser removido.</param>
    /// <returns>
    /// 204 No Content se o usuário for removido com sucesso.
    /// 404 Not Found se o usuário com o ID fornecido não for encontrado.
    /// 400 Bad Request se os dados fornecidos forem inválido
    /// 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeletarUsuario(int id)
    {
        try
        {
            await _servico.Remover(id);
            
            return NoContent();
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            _logger.LogError(ex, ex.Message);

            return NotFound($"Nenhum usuário encontrado com o id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return  Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    /// Autentica um usuário com email e senha.
    /// </summary>
    /// <param name="usuarioLoginDto">Os dados de login do usuário (email e senha).</param>
    /// <returns>
    /// 200 OK com os detalhes do usuário autenticado.
    /// 401 Unauthorized se a autenticação falhar.
    /// 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
    {
        try
        {
            var resultado = await _servico.AutenticarLogin(usuarioLoginDto);

            return Ok(resultado);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return Unauthorized("E-mail ou Senha inválidos.");
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return Unauthorized("E-mail ou Senha inválidos.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return Problem("Erro interno do servidor.");
        }
        
    }
}