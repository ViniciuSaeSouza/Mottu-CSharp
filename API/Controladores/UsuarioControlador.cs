using Aplicacao.DTOs.Usuario;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Abstracoes;

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
    [HttpGet(Name = nameof(ListarUsuarios))]
    [ProducesResponseType(typeof(Recurso<IEnumerable<UsuarioLeituraDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<IEnumerable<UsuarioLeituraDto>>>> ListarUsuarios()
    {
        try
        {
            var usuarios = await _servico.ObterTodos();

            var recurso = new Recurso<IEnumerable<UsuarioLeituraDto>>
            {
                Dados = usuarios,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Link(nameof(ListarUsuarios), null) ?? string.Empty, Method = "GET" },
                    new Link { Rel = "create", Href = Url.Link(nameof(CriarUsuario), null) ?? string.Empty, Method = "POST" }
                }
            };

            return Ok(recurso);
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
    [HttpGet("{id:int}", Name = nameof(ObterUsuarioPorId))]
    [ProducesResponseType(typeof(Recurso<UsuarioLeituraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<UsuarioLeituraDto>>> ObterUsuarioPorId(int id)
    {
        try
        {
            var usuario = await _servico.ObterPorId(id);

            var recurso = new Recurso<UsuarioLeituraDto>
            {
                Dados = usuario,
                Links = CriarLinks(usuario)
            };

            return Ok(recurso);
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
    [HttpPost(Name = nameof(CriarUsuario))]
    [ProducesResponseType(typeof(Recurso<UsuarioLeituraDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<UsuarioLeituraDto>>> CriarUsuario([FromBody] UsuarioCriarDto dto)
    {
        try
        {
            var usuario = await _servico.Criar(dto);

            var recurso = new Recurso<UsuarioLeituraDto>
            {
                Dados = usuario,
                Links = CriarLinks(usuario)
            };

            return CreatedAtAction(nameof(ObterUsuarioPorId), new { id = usuario.IdUsuario }, recurso);
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
    [HttpPut("{id:int}", Name = nameof(AtualizarUsuario))]
    [ProducesResponseType(typeof(Recurso<UsuarioLeituraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<UsuarioLeituraDto>>> AtualizarUsuario(int id, [FromBody] UsuarioAtualizarDto dto)
    {
        try
        {
            var usuario = await _servico.Atualizar(id, dto);

            var recurso = new Recurso<UsuarioLeituraDto>
            {
                Dados = usuario,
                Links = CriarLinks(usuario)
            };

            return Ok(recurso);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest("Dados de requisição inválidos.");
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
    /// 400 Bad Request se os dados fornecidos forem inválidos
    /// 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// </returns>
    [HttpDelete("{id:int}", Name = nameof(DeletarUsuario))]
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
    [HttpPost("login", Name = nameof(Login))]
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

    private List<Link> CriarLinks(UsuarioLeituraDto usuario)
    {
        var links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(ObterUsuarioPorId), new { id = usuario.IdUsuario }) ?? string.Empty, Method = "GET" },
            new Link { Rel = "update", Href = Url.Link(nameof(AtualizarUsuario), new { id = usuario.IdUsuario }) ?? string.Empty, Method = "PUT" },
            new Link { Rel = "delete", Href = Url.Link(nameof(DeletarUsuario), new { id = usuario.IdUsuario }) ?? string.Empty, Method = "DELETE" },
            new Link { Rel = "collection", Href = Url.Link(nameof(ListarUsuarios), null) ?? string.Empty, Method = "GET" }
        };

        return links;
    }
}