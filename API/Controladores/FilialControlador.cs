using API.Aplicacao.Repositorios;
using API.Application;
using Aplicacao.DTOs.Filial;
using Aplicacao.DTOs.Moto;
using Aplicacao.Servicos;
using Dominio.Persistencia;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;


// TODO: Remover lógicas de validação do controlador e jogar para a camada SERVICE

[Route("api/[controller]")]
[ApiController]
[Tags("Filiais")]
public class FilialControlador : ControllerBase
{

    private readonly FilialServico _filialServico;

    public FilialControlador(FilialServico filialServico)
    {
        _filialServico = filialServico;
    }


    /// <summary>
    /// Obtém uma lista de todas as filiais sem as motos associadas.
    /// </summary>
    /// <returns>
    /// Retorna uma lista de objetos FiliaisReadDto representando as filiais sem as motos associadas.
    /// Retorna 200 OK se as filiais forem encontradas.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<FilialLeituraDto>>> GetFiliais()
    {
        var filiais = await _filialServico.ObterTodos();
        return Ok(filiais);
    }


    /// <summary>
    ///  Retorna uma filial específica pelo Id passado por parâmetro junto com suas motos relacionadas.
    /// </summary>
    /// <param name="id">
    /// ID da filial a ser retornada.
    /// </param>
    /// <returns>
    /// Retorna um objeto FilialReadDto representando a filial encontrada.
    /// Retorna 200 OK se a filial for encontrada, ou 404 Not Found se não houver filial com o ID fornecido.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FilialLeituraDto>> GetFilial(int id)
    {
        try
        {
            var filial = await _filialServico.ObterPorId(id);
            return Ok(filial);
        }
        catch
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Cria uma nova filial no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="filialCreateDto">
    /// Dto de criação da filial, contendo os dados necessários para criar uma nova filial.
    /// </param>
    /// <returns>
    /// Retorna um objeto FilialReadDto representando a filial criada.
    /// Retorna 201 Created se a filial for criada com sucesso, ou 400 se o objeto filialCreateDto não for passado corretamente no corpo.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<FilialLeituraDto>> CriarFilial([FromBody] FilialCriarDto filialCreateDto)
    {
        try
        {
            var filial = await _filialServico.Criar(filialCreateDto);
            return CreatedAtAction(nameof(GetFilial), new { id = filial.Id }, filial);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///  Altera um ou mais dados de uma filial existente no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="id">ID da filial a ser atualizada</param>
    /// <param name="filialUpdateDto">Objeto contendo um ou mais atributos de uma filial</param>
    /// <returns>
    /// Retorna um objeto FilialReadDto representando a filial atualizada.
    /// Retorna 200 OK se a filial for atualizada com sucesso, ou 404 Not Found se não houver filial com o ID fornecido.
    /// REtorna 400 Bad Request se o objeto filialUpdateDto não for passado corretamente no corpo.
    /// </returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PatchFilial(int id, [FromBody] FilialAtualizarDto filialUpdateDto)
    {
        try
        {
            var filialAtualizada = await _filialServico.Atualizar(id, filialUpdateDto);
            return Ok(filialAtualizada);
        }
        catch
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Deleta uma filial existente no banco de dados com o ID fornecido.
    /// </summary>
    /// <param name="id">
    /// ID da filial a ser excluída.
    /// </param>
    /// <returns>
    /// REtona 204 No Content se a filial for excluída com sucesso, ou 404 Not Found se não houver filial com o ID fornecido.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeletaFilial(int id)
    {
        try
        {
            await _filialServico.Remover(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }
}
