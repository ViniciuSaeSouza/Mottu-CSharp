using Aplicacao.DTOs.Filial;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

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
        try
        {
            var filiais = await _filialServico.ObterTodos();
            return Ok(filiais);
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar todas as filiais no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao buscar filiais: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
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
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Filial de id {id} não encontrada: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = $"Nenhuma filial encontrada para o id {id}" });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar filial de id {id} no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao buscar filial {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
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
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao criar filial: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao criar filial: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao criar filial: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
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
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Filial de id {id} não encontrada para atualização: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao atualizar filial {id}: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao atualizar filial {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao atualizar filial {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
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
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Filial de id {id} não encontrada para remoção: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao remover filial {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao remover filial {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }
}
