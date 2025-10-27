using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/modelos-moto")]
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
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public IActionResult Listar()
    {
        try
        {
            var modelos = Enum
                .GetValues(enumType: typeof(ModeloMotoEnum))
                .Cast<ModeloMotoEnum>()
                .Select(m => new { id = (int)m, nome = m.ToString() });

            return Ok(modelos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Problem("Erro interno do servidor.");
        }
    }
}