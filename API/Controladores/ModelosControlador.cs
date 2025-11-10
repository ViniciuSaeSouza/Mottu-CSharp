using Asp.Versioning;
using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Aplicacao.Abstracoes;

namespace API.Controladores;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/modelos-moto")]
[Tags("Modelos das Motos")]
[AllowAnonymous]
public class ModelosControlador : ControllerBase
{
    private readonly ILogger<ModelosControlador> _logger;

    public ModelosControlador(ILogger<ModelosControlador>  logger)
    {
        _logger = logger;
    }
    
    
    /// <summary>
    /// Lista todos os modelos de moto disponíveis.
    /// </summary>
    /// <returns>
    /// Uma lista de objetos representando os modelos de moto, cada um contendo um id e um nome.
    /// 200 em caso de sucesso
    /// 500 em caso de erro.
    /// </returns>
    [HttpGet(Name = nameof(Listar))]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public ActionResult<Recurso<IEnumerable<object>>> Listar()
    {
        try
        {
            var modelos = Enum
                .GetValues(enumType: typeof(ModeloMotoEnum))
                .Cast<ModeloMotoEnum>()
                .Select(m => new { id = (int)m, nome = m.ToString() });

            var recurso = new Recurso<IEnumerable<object>>
            {
                Dados = modelos,
                Links = CriarLinksColecao()
            };

            return Ok(recurso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }

    private List<Link> CriarLinksColecao()
    {
        return new List<Link>
        {
            new Link { Rel = "self", Href = Url.Link(nameof(Listar), null) ?? string.Empty, Method = "GET" }
        };
    }
}