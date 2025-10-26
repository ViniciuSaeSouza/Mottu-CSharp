using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Abstracoes;

namespace API.Controladores;

[ApiController]
[Route("api/zonas")]
[Tags("Zonas")]
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
    [HttpGet(Name = nameof(ObterZonas))]
    [ProducesResponseType(typeof(Recurso<IEnumerable<object>>), StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<object>> ObterZonas()
    {
        try
        {
            var zonas = Enum.GetValues(typeof(ZonaEnum))
                .Cast<ZonaEnum>()
                .Select(z => new { Id = (int)z, Nome = z.ToString() })
                .ToList();

            var recurso = new Recurso<IEnumerable<object>>
            {
                Dados = zonas,
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Link(nameof(ObterZonas), null) ?? string.Empty, Method = "GET" }
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
}