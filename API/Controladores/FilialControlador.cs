using API.Aplicacao.Repositorios;
using API.Application;
using Aplicacao.DTOs.Filial;
using Aplicacao.DTOs.Moto;
using Dominio.Persistencia;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;


// TODO: Remover lógicas de validação do controlador e jogar para a camada SERVICE

[Route("api/[controller]")]
[ApiController]
[Tags("Filiais")]
public class FilialControlador : ControllerBase
{

    private readonly FilialRepositorio _repositorio;
    private readonly MotoRepositorio _motoRepositorio;
    public FilialControlador(FilialRepositorio repositorio, MotoRepositorio motoRepositorio)
    {
        _repositorio = repositorio;
        _motoRepositorio = motoRepositorio;
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
        var filiais = await _repositorio.ObterTodos();
        // TODO: Utilizar classe FilialLeituraDTO
        var filiaisDto = filiais.Select(f => new FiliaisLeituraDto
        {
            Id = f.Id,
            Nome = f.Nome,
            Endereco = f.Endereco,
        }).ToList();

        return Ok(filiaisDto);
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
        var filial = await _repositorio.ObterPorId(id);

        if (filial == null)
        {
            return NotFound();
        }

        var filialDto = new FilialLeituraDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoLeituraDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo.ToString().ToUpper(),
                NomeFilial = filial.Nome
            }).ToList()
        };

        return Ok(filialDto);
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
        if (filialCreateDto == null)
        {
            return BadRequest("Filial não pode ser nula.");
        }

        var filial = new Filial(filialCreateDto.Nome, filialCreateDto.Endereco);

        await _repositorio.Adicionar(filial);


        // TODO: VERIFICAR SE TRAZ UMA LISTA VAZIA EM 'MOTOS'
        var filialReadDto = new FilialLeituraDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco
        };
        return CreatedAtAction(nameof(GetFilial), new { id = filial.Id }, filialReadDto);
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
        var filial = await _repositorio.ObterPorId(id);

        if (filial == null)
        {
            return NotFound();
        }
        if (filialUpdateDto.Endereco != null)
        {
            filial.AlterarEndereco(filialUpdateDto.Endereco);
        }
        if (filialUpdateDto.Nome != null)
        {
            filial.AlterarNome(filialUpdateDto.Nome);
        }

        await _repositorio.Atualizar(filial);

        var filialReadDto = new FilialLeituraDto
        {
            Id = filial.Id,
            Nome = filial.Nome,
            Endereco = filial.Endereco,
            Motos = filial.Motos.Select(m => new MotoLeituraDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo.ToString().ToUpper(),
                NomeFilial = filial.Nome
            }).ToList()
        };
        return Ok(filialReadDto);
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
        var filial = await _repositorio.ObterPorId(id);
        if (filial == null)
        {
            return NotFound();
        }

        await _repositorio.Remover(filial);

        return NoContent();
    }
}
