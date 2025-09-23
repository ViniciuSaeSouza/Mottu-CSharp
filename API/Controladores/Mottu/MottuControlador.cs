using Aplicacao.DTOs.Moto.Mottu;
using Aplicacao.Servicos.Mottu;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores.Mottu;

[ApiController]
[Route("api/mottu/motos")]
public class MottuControlador
{
    private readonly MotoMottuServico _servico;

    public MottuControlador(MotoMottuServico servico)
    {
        _servico = servico;
    }

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