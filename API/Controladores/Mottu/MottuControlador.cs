using Aplicacao.DTOs.Moto.Mottu;
using Aplicacao.Servicos.Mottu;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores.Mottu;

[ApiController]
[Route("api/mottu/motos")]
[Tags("Mottu - Motos")]
public class MottuControlador
{
    private readonly MotoMottuServico _servico;

    public MottuControlador(MotoMottuServico servico)
    {
        _servico = servico;
    }
    
    /// <summary>
    /// Retorna a lista de motos cadastradas no sistema da mottu (mockado).
    /// </summary>
    /// <returns>
    /// Retorna 200 OK com a lista de motos cadastradas.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ListarMotos()
    {
        try
        {
            var motos = await _servico.ObterTodosAssincrono();
            return new OkObjectResult(motos);
        }
        catch (ExcecaoBancoDados ex)
        {
            return new ObjectResult("Serviço de banco de dados indisponível")
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable
            };
        }
        catch (Exception ex)
        {
            return new ObjectResult("Erro interno do servidor")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
    
    
    /// <summary>
    /// Retorna uma moto específica pelo ID passado por parâmetro.
    /// </summary>
    /// <param name="id"> ID da moto a ser buscada </param>
    /// <returns>
    /// Retorna 200 OK com a moto encontrada ou 404 Not Found se a moto não for encontrada.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> ObterPorId(int id)
    {
        try
        {
            var moto = await _servico.ObterPorIdAssincrono(id);
            return moto is not null
                ? new OkObjectResult(moto)
                : new NotFoundResult();
        }
        catch (ExcecaoBancoDados ex)
        {
            return new ObjectResult("Serviço de banco de dados indisponível")
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable
            };
        }
        catch (Exception ex)
        {
            return new ObjectResult("Erro interno do servidor")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}