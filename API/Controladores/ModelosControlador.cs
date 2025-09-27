using Dominio.Enumeradores;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;


[ApiController]
[Route("api/modelos-moto")]
[Tags("Modelos das Motos")]
public class ModelosControlador : ControllerBase
{
    
    /// <summary>
    /// Lista todos os modelos de moto disponíveis.
    /// </summary>
    /// <returns>
    /// Uma lista de objetos representando os modelos de moto, cada um contendo um id e um nome.
    /// Em caso de erro, retorna um status 500 com uma mensagem de erro.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
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
            Console.WriteLine("Erro ao listar modelos de moto: " + ex.StackTrace);
            return StatusCode(statusCode: 500, value: new { mensagem = "Ocorreu um erro ao listar os modelos de moto.", detalhes = ex.Message });
        }
    }
}