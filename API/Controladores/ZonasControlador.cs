using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[Route("api/zonas")]
[Tags("Zonas")]
public class ZonasControlador : ControllerBase  
{
    /// <summary>
    /// Obtém a lista de zonas disponíveis.
    /// </summary>
    /// <returns>
    /// Uma lista de objetos representando as zonas, cada um contendo um Id e um Nome.
    /// Em caso de erro, retorna um status 500 com uma mensagem de erro.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<object>> ObterZonas()
    {
        try
        {
            var zonas = Enum.GetValues(typeof(ZonaEnum))
                .Cast<ZonaEnum>()
                .Select(z => new { Id = z, Nome = z.ToString() })
                .ToList();

            return Ok(zonas);

        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao listar zonas: " + ex.StackTrace);
            return StatusCode(statusCode: 500, value: new { mensagem = "Ocorreu um erro ao listar as zonas.", detalhes = ex.Message });
        }
    }
}