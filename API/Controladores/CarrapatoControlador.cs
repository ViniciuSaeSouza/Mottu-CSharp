using Aplicacao.DTOs.Carrapato;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Abstracoes;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controladores;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/carrapatos")]
[Tags("Carrapatos")]
[Authorize]
public class CarrapatoControlador : ControllerBase
{
    private readonly CarrapatoServico _servico;
    private readonly ILogger<CarrapatoControlador> _logger;

    public CarrapatoControlador(CarrapatoServico servico, ILogger<CarrapatoControlador>  logger)
    {
        _servico = servico;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os carrapatos cadastrados (paginado).
    /// </summary>
    /// <param name="pagina">Número da página (padrão: 1).</param>
    /// <param name="tamanhoPagina">Tamanho da página (padrão: 10).</param>
    /// <returns>
    /// 200 OK com a página de carrapatos e links HATEOAS.
    /// </returns>
    [HttpGet(Name = nameof(GetTodos))]
    [ProducesResponseType(typeof(Recurso<IResultadoPaginado<CarrapatoLeituraDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<IResultadoPaginado<CarrapatoLeituraDto>>>> GetTodos([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
    {
        try
        {
            var carrapatos = await _servico.ObterTodosPaginado(pagina, tamanhoPagina);

            var recurso = new Recurso<IResultadoPaginado<CarrapatoLeituraDto>>
            {
                Dados = carrapatos,
                Links = this.CriarLinksPaginados(carrapatos, tamanhoPagina, pagina)
            };

            return Ok(recurso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    /// Busca um carrapato pelo id.
    /// </summary>
    /// <param name="id">Id do carrapato</param>
    /// <returns>Retorna 200 OK com o carrapato, 404 Not Found se não existir, 500 ou 503 em caso de erro.</returns>
    [HttpGet("{id}", Name = nameof(GetPorId))]
    [ProducesResponseType(typeof(Recurso<CarrapatoLeituraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<CarrapatoLeituraDto>>> GetPorId(int id)
    {
        try
        {
            var carrapato = await _servico.ObterPorId(id);

            var recurso = new Recurso<CarrapatoLeituraDto>
            {
                Dados = carrapato,
                Links = this.CriarLinks(carrapato)
            };

            return Ok(recurso);
        }
        catch (ExcecaoEntidadeNaoEncontrada)
        {
            return NotFound();
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest($"Dados da requisição inválidos: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    /// Cria um novo carrapato.
    /// </summary>
    /// <param name="dto">Dados para criação do carrapato</param>
    /// <remarks>
    /// Exemplo de payload:
    /// <example>
    /// {
    ///   "CodigoSerial": "ABC123",
    ///   "IdPatio": 42
    /// }
    /// </example>
    /// </remarks>
    [HttpPost(Name = nameof(Criar))]
    [ProducesResponseType(typeof(Recurso<CarrapatoLeituraDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<CarrapatoLeituraDto>>> Criar([FromBody] CarrapatoCriarDto dto)
    {
        try
        {
            var criado = await _servico.Criar(dto);

            var recurso = new Recurso<CarrapatoLeituraDto>
            {
                Dados = criado,
                Links = this.CriarLinks(criado)
            };

            return CreatedAtAction(nameof(GetPorId), new { id = criado.Id }, recurso);
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest($"Dados da requisição inválidos: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    /// Atualiza um carrapato existente.
    /// </summary>
    /// <param name="id">Id do carrapato</param>
    /// <param name="dto">Dados para atualização</param>
    /// <remarks>
    /// Exemplo de payload para atualização:
    /// <example>
    /// {
    ///   "CodigoSerial": "ABC123XYZ",
    ///   "IdPatio": 42
    /// }
    /// </example>
    /// </remarks>
    [HttpPut("{id}", Name = nameof(Atualizar))]
    [ProducesResponseType(typeof(Recurso<CarrapatoLeituraDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Recurso<CarrapatoLeituraDto>>> Atualizar(int id, [FromBody] CarrapatoCriarDto dto)
    {
        try
        {
            var carrapatoAtualizado = await _servico.Atualizar(id, dto);

            var recurso = new Recurso<CarrapatoLeituraDto>
            {
                Dados = carrapatoAtualizado,
                Links = this.CriarLinks(carrapatoAtualizado)
            };

            return Ok(recurso);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            _logger.LogError(ex, ex.Message);

            return NotFound($"Não foi possível encontrar nenhum carrapato para o id {id}");
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest($"Dados da requisição inválidos: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    /// <summary>
    /// Remove um carrapato pelo id.
    /// </summary>
    /// <param name="id">Id do carrapato</param>
    /// <returns>
    /// Retorna 204 No Content se removido
    /// 404 Not Found se não existir
    /// 500 em caso de erro.
    /// </returns>
    [HttpDelete("{id}", Name = nameof(Remover))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Remover(int id)
    {
        try
        {
            await _servico.Remover(id);

            return NoContent();
        }
        catch (ExcecaoDominio ex)
        {
            _logger.LogError(ex, ex.Message);

            return BadRequest($"Dados da requisição inválidos: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    private List<Link> CriarLinks(CarrapatoLeituraDto carrapato)
    {
        var links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetPorId), new { id = carrapato.Id }) ?? string.Empty, Method = "GET" },
            new Link { Rel = "update", Href = Url.Link(nameof(Atualizar), new { id = carrapato.Id }) ?? string.Empty, Method = "PUT" },
            new Link { Rel = "delete", Href = Url.Link(nameof(Remover), new { id = carrapato.Id }) ?? string.Empty, Method = "DELETE" },
            new Link { Rel = "collection", Href = Url.Link(nameof(GetTodos), null) ?? string.Empty, Method = "GET" }
        };

        return links;
    }

    private List<Link> CriarLinksPaginados(IResultadoPaginado<CarrapatoLeituraDto> carrapatosPaginados, int tamanho, int pagina)
    {
        var links = new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(GetTodos), new { pagina, tamanhoPagina = tamanho }) ?? string.Empty, Method = "GET" },
            new Link { Rel = "first", Href = Url.Link(nameof(GetTodos), new { pagina = 1, tamanhoPagina = tamanho }) ?? string.Empty, Method = "GET" }
        };

        var paginaAtual = carrapatosPaginados.Pagina;
        var totalPaginas = carrapatosPaginados.TotalPaginas;

        if (paginaAtual < totalPaginas)
        {
            links.Add(new Link { Rel = "next", Href = Url.Link(nameof(GetTodos), new { pagina = paginaAtual + 1, tamanhoPagina = tamanho }) ?? string.Empty, Method = "GET" });
        }

        if (paginaAtual > 1)
        {
            links.Add(new Link { Rel = "prev", Href = Url.Link(nameof(GetTodos), new { pagina = paginaAtual - 1, tamanhoPagina = tamanho }) ?? string.Empty, Method = "GET" });
        }

        links.Add(new Link { Rel = "last", Href = Url.Link(nameof(GetTodos), new { pagina = totalPaginas, tamanhoPagina = tamanho }) ?? string.Empty, Method = "GET" });

        return links;
    }
}
