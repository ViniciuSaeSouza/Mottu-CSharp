using Aplicacao.DTOs.Patio;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Abstracoes;

namespace API.Controladores;

[Route("api/patios")]
[ApiController]
[Tags("Patios")]
public class PatioControlador : ControllerBase
{
    private readonly PatioServico _patioServico;
    private readonly ILogger<PatioControlador> _logger;

    public PatioControlador(PatioServico patioServico, ILogger<PatioControlador>  logger)
    {
        _patioServico = patioServico;
        _logger = logger;
    }

    /// <summary>
    /// Obtém uma lista de todos os pátios e suas motos associadas.
    /// </summary>
    /// <returns>
    /// Retorna uma lista de objetos PatioLeituraDto representando os pátios, suas motos e usuarios associados.
    /// 200 OK se os pátios forem encontrados.
    /// 500 em caso de erro.
    /// </returns>
    [HttpGet(Name = nameof(GetPatios))]
    [ProducesResponseType(typeof(Recurso<IEnumerable<PatioLeituraDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<IEnumerable<PatioLeituraDto>>>> GetPatios()
    {
        try
        {
            var patios = await _patioServico.ObterTodos();

            var recurso = new Recurso<IEnumerable<PatioLeituraDto>>
            {
                Dados = patios,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Link(nameof(GetPatios), null) ?? string.Empty, Method = "GET" },
                    new Link { Rel = "create", Href = Url.Link(nameof(CriarPatio), null) ?? string.Empty, Method = "POST" }
                }
            };

            return Ok(recurso);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest("Dados de requisição inválidos");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    ///  Retorna um pátio específico pelo Id passado por parâmetro com suas motos relacionadas.
    /// </summary>
    /// <param name="id">
    /// ID do pátio a ser retornado.
    /// </param>
    /// <returns>
    /// Retorna um objeto PatioLeituraDto representando o pátio encontrado.
    /// 200 OK se o pátio for encontrado, ou 404 Not Found se não houver patio com o ID fornecido.
    /// 400 Bad Request se o ID for inválido (não for um número inteiro).
    /// 404 Not Found em caso de pátio não encontrado.
    /// 500 em caso de erro.
    /// </returns>
    [HttpGet("{id}", Name = nameof(GetPatio))]
    [ProducesResponseType(typeof(Recurso<PatioLeituraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<PatioLeituraDto>>> GetPatio(int id)
    {
        try
        {
            var patio = await _patioServico.ObterPorId(id);

            var recurso = new Recurso<PatioLeituraDto>
            {
                Dados = patio,
                Links = CriarLinks(patio)
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

            return NotFound($"Nenhum pátio encontrado para o id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");    
        }
    }

    /// <summary>
    /// Cria um novo pátio no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="patioCreateDto">
    /// Dto de criação do pátio, contendo os dados necessários para criar um novo pátio.
    /// </param>
    /// <returns>
    /// Retorna um objeto PatioLeituraDto representando o pátio criado.
    /// 201 Created se o pátio for criado com sucesso
    /// 400 se o objeto patioCreateDto não for passado corretamente no corpo.
    /// 500 em caso de erro.
    /// </returns>
    [HttpPost(Name = nameof(CriarPatio))]
    [ProducesResponseType(typeof(Recurso<PatioLeituraDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<PatioLeituraDto>>> CriarPatio([FromBody] PatioCriarDto patioCreateDto)
    {
        try
        {
            var patio = await _patioServico.Criar(patioCreateDto);

            var recurso = new Recurso<PatioLeituraDto>
            {
                Dados = patio,
                Links = CriarLinks(patio)
            };

            return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, recurso);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest("Dados de requisição inválidos.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    ///  Altera um ou mais dados de um pátio existente no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="id">ID do pátio a ser atualizado</param>
    /// <param name="patioUpdateDto">Objeto contendo um ou mais atributos de um patio</param>
    /// <returns>
    /// Retorna um objeto PatioLeituraDto representando o pátio atualizado.
    /// 200 OK se o pátio for atualizado com sucesso.
    /// 404 Not Found se não houver patio com o ID fornecido.
    /// 400 Bad Request se o objeto patioUpdateDto não for passado corretamente no corpo.
    /// 500 em caso de erro.
    /// </returns>
    [HttpPatch("{id}", Name = nameof(PatchPatio))]
    [ProducesResponseType(typeof(Recurso<PatioLeituraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PatchPatio(int id, [FromBody] PatioAtualizarDto patioUpdateDto)
    {
        try
        {
            var patioAtualizado = await _patioServico.Atualizar(id, patioUpdateDto);

            var recurso = new Recurso<PatioLeituraDto>
            {
                Dados = patioAtualizado,
                Links = CriarLinks(patioAtualizado)
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

            return NotFound($"Nenhum pátio encontrado com o id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Deleta um pátio existente no banco de dados com o ID fornecido.
    /// </summary>
    /// <param name="id">
    /// ID do patio a ser excluído.
    /// </param>
    /// <returns>
    /// Retorna 204 No Content se o pátio for excluído com sucesso.
    /// 404 Not Found se não houver patio com o ID fornecido.
    /// Retorna 400 Bad Request se o ID for inválido (não for um número inteiro).
    /// 500 em caso de erro.
    /// </returns>
    [HttpDelete("{id}", Name = nameof(DeletaPatio))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletaPatio(int id)
    {
        try
        {
            await _patioServico.Remover(id);

            return NoContent();
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest("Dados de requisição inválidos");
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return NotFound($"Nenhum pátio encontrado com o id {id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return  Problem("Erro interno do servidor");
        }
    }
}

    private List<Link> CriarLinks(PatioLeituraDto patio)
    {
        var links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetPatio), new { id = patio.Id }) ?? string.Empty, Method = "GET" },
            new Link { Rel = "update", Href = Url.Link(nameof(PatchPatio), new { id = patio.Id }) ?? string.Empty, Method = "PATCH" },
            new Link { Rel = "delete", Href = Url.Link(nameof(DeletaPatio), new { id = patio.Id }) ?? string.Empty, Method = "DELETE" },
            new Link { Rel = "collection", Href = Url.Link(nameof(GetPatios), null) ?? string.Empty, Method = "GET" }
        };

        return links;
    }
}
