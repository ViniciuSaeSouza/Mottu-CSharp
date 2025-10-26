using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;
using Aplicacao.Abstracoes;

namespace API.Controladores;


[ApiController]
[Route("api/modelos-moto")]
[Tags("Modelos das Motos")]
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
    [ProducesResponseType(typeof(Recurso<IEnumerable<object>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Listar()
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
                Links = new List<Link>
                {
                    new Link { Rel = "self", Href = Url.Link(nameof(Listar), null) ?? string.Empty, Method = "GET" }
                }
            };

            return Ok(recurso);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }
}