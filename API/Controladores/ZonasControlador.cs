using Asp.Versioning;
using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplicacao.Abstracoes;

namespace API.Controladores;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/zonas")]
[Tags("Zonas")]
[AllowAnonymous]
public class ZonasControlador : ControllerBase  
{
    private readonly ILogger<ZonasControlador> _logger;

    public ZonasControlador(ILogger<ZonasControlador>  logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Obtém a lista de zonas disponíveis.
    /// </summary>
    /// <returns>
    /// Uma lista de objetos representando as zonas, cada um contendo um Id e um Nome.
    /// Em caso de erro, retorna um status 500 com uma mensagem de erro.
    /// </returns>
    [HttpGet(Name = nameof(ObterZonas))]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public ActionResult<Recurso<IEnumerable<object>>> ObterZonas()
    {
        try
        {
            var zonas = Enum.GetValues(typeof(ZonaEnum))
                .Cast<ZonaEnum>()
                .Select(z => new { Id = z, Nome = z.ToString() })
                .ToList();

            var recurso = new Recurso<IEnumerable<object>>
            {
                Dados = zonas,
                Links = CriarLinksColecao()
            };

            return Ok(recurso);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor");
        }
    }

    private List<Link> CriarLinksColecao()
    {
        return new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(ObterZonas), null) ?? string.Empty, Method = "GET" }
        };
    }
}