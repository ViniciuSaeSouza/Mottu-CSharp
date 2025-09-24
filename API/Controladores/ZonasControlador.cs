using Dominio.Enumeradores;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[Route("api/zonas")]
[Tags("Zonas")]
public class ZonasControlador
{
    [HttpGet]
    public ActionResult<IEnumerable<object>> ObterZonas()
    {
        var zonas = Enum.GetValues(typeof(ZonaEnum))
            .Cast<ZonaEnum>()
            .Select(z => new { Id = z, Nome = z.ToString() })
            .ToList();
        
        return new ObjectResult(zonas);
    }
}