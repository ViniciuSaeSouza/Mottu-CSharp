using API.Aplicacao.Repositorios;
using API.Application;
using Aplicacao.DTOs.Moto;
using Aplicacao.Servicos;
using Dominio.Enumeradores;
using Dominio.Excecao;
using Dominio.Persistencia;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;


// TODO: Remover lógicas de validação do controlador e jogar para a camada SERVICE

[Route("api/[controller]")]
[ApiController]
[Tags("Motos")]
public class MotosControlador : ControllerBase
{
    private readonly MotoServico _motoServico;

    public MotosControlador(MotoServico motoServico)
    {
        _motoServico = motoServico;
    }


    /// <summary>
    /// Retorna a lista de motos cadastradas no sistema.
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
    public async Task<ActionResult<IEnumerable<Moto>>> GetMotos()
    {
        var motos = await _motoServico.ObterTodos();
        return Ok(motos);
    }

    /// <summary>
    /// Retorna uma moto específica pelo ID passado por parâmetro.
    /// </summary>
    /// <param name="id"> ID da moto a ser buscada </param>
    /// <returns>
    /// Retorna 200 OK com a moto encontrada ou 404 Not Found se a moto não for encontrada.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Moto>> GetMoto(int id)
    {
        {
            try
            {
                var moto = await _motoServico.ObterPorId(id);
                return Ok(moto);
            }
            catch
            {
                return NotFound();
            }
        }
    }

    /// <summary>
    /// Insere uma nova moto no sistema.
    /// </summary>
    /// <param name="motoDto"> 
    /// Objeto que representa o cadastro de uma nova moto.
    /// </param>
    /// <returns>
    /// Retorna 201 Created com a moto criada ou 400 Bad Request se o corpo da requisição não for válido.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]

    public async Task<ActionResult<Moto>> CriarMoto([FromBody] MotoCriarDto motoDto)
    {
            try
            {
                var moto = await _motoServico.Criar(motoDto);
                return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
            }
            catch (ExcecaoDominio ex)
            {
                return BadRequest(ex.Message);
            }
        }


    /// <summary>
    /// Retorna a moto com as informações atualizadas.
    /// </summary>
    /// <param name="id"> ID da moto a ser atualizada </param>
    /// <param name="motoUpdateDto">Objeto contendo um ou mais atributos de moto a serem atualizados</param>
    /// <returns>
    /// Retorna 200 OK com a moto atualizada ou 404 Not Found se a moto ou nova filial não for encontrada.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Moto>> PatchMoto(int id, [FromBody] MotoAtualizarDto motoUpdateDto)
    {
        try
        {
            var motoAtualizada = await _motoServico.Atualizar(id, motoUpdateDto);
            return Ok(motoAtualizada);
        }
        catch
        {
            return NotFound();
        }
    }


    /// <summary>
    ///  Retorna código 204 confirmando a exclusão da moto com o ID passado por parâmetro.
    /// </summary>
    /// <param name="id"> ID da moto a ser excluída </param>
    /// <returns>
    /// Retorna código 204 No Content se a moto for excluída com sucesso ou 404 Not Found se a moto não for encontrada.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeletarMoto(int id)
    {
        try
        {
            await _motoServico.Remover(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}