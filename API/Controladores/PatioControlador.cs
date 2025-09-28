using Aplicacao.DTOs.Patio;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[Route("api/patios")]
[ApiController]
[Tags("Patios")]
public class PatioControlador : ControllerBase
{
    private readonly PatioServico _patioServico;

    public PatioControlador(PatioServico patioServico)
    {
        _patioServico = patioServico;
    }

    /// <summary>
    /// Obtém uma lista de todos os patios e suas motos associadas.
    /// </summary>
    /// <returns>
    /// Retorna uma lista de objetos PatioLeituraDto representando os patios, suas motos e usuarios associados.
    /// Retorna 200 OK se os patios forem encontrados.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<PatioLeituraDto>>> GetPatios()
    {
        try
        {
            var patios = await _patioServico.ObterTodos();
            return Ok(patios);
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar todos os patios no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao buscar patios: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    ///  Retorna um patio específico pelo Id passado por parâmetro junto com suas motos relacionadas.
    /// </summary>
    /// <param name="id">
    /// ID do patio a ser retornado.
    /// </param>
    /// <returns>
    /// Retorna um objeto PatioLeituraDto representando o patio encontrado.
    /// Retorna 200 OK se o patio for encontrado, ou 404 Not Found se não houver patio com o ID fornecido.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// </returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PatioLeituraDto>> GetPatio(int id)
    {
        try
        {
            var patio = await _patioServico.ObterPorId(id);
            return Ok(patio);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Patio de id {id} não encontrado: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = $"Nenhum patio encontrado para o id {id}" });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar patio de id {id} no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao buscar patio {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Cria um novo patio no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="patioCreateDto">
    /// Dto de criação do patio, contendo os dados necessários para criar um novo patio.
    /// </param>
    /// <returns>
    /// Retorna um objeto PatioLeituraDto representando o patio criado.
    /// Retorna 201 Created se o patio for criado com sucesso, ou 400 se o objeto patioCreateDto não for passado corretamente no corpo.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<PatioLeituraDto>> CriarPatio([FromBody] PatioCriarDto patioCreateDto)
    {
        try
        {
            var patio = await _patioServico.Criar(patioCreateDto);
            return CreatedAtAction(nameof(GetPatio), new { id = patio.Id }, patio);
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao criar patio: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao criar patio: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao criar patio: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    ///  Altera um ou mais dados de um patio existente no banco de dados com os dados fornecidos no DTO.
    /// </summary>
    /// <param name="id">ID do patio a ser atualizado</param>
    /// <param name="patioUpdateDto">Objeto contendo um ou mais atributos de um patio</param>
    /// <returns>
    /// Retorna um objeto PatioLeituraDto representando o patio atualizado.
    /// Retorna 200 OK se o patio for atualizado com sucesso, ou 404 Not Found se não houver patio com o ID fornecido.
    /// Retorna 400 Bad Request se o objeto patioUpdateDto não for passado corretamente no corpo.
    /// </returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PatchPatio(int id, [FromBody] PatioAtualizarDto patioUpdateDto)
    {
        try
        {
            var patioAtualizado = await _patioServico.Atualizar(id, patioUpdateDto);
            return Ok(patioAtualizado);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Patio de id {id} não encontrado para atualização: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao atualizar patio {id}: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao atualizar patio {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao atualizar patio {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Deleta um patio existente no banco de dados com o ID fornecido.
    /// </summary>
    /// <param name="id">
    /// ID do patio a ser excluído.
    /// </param>
    /// <returns>
    /// Retorna 204 No Content se o patio for excluído com sucesso, ou 404 Not Found se não houver patio com o ID fornecido.
    /// Retorna 400 Bad Request se o ID não for válido (não for um número inteiro).
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> DeletaPatio(int id)
    {
        try
        {
            await _patioServico.Remover(id);
            return NoContent();
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Patio de id {id} não encontrado para remoção: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao remover patio {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao remover patio {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }
}
