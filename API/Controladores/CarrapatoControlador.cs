using Aplicacao.DTOs.Carrapato;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[Route("api/carrapatos")]
[Tags("Carrapatos")]
public class CarrapatoControlador : ControllerBase
{
    private readonly CarrapatoServico _servico;

    public CarrapatoControlador(CarrapatoServico servico)
    {
        _servico = servico;
    }

    /// <summary>
    /// Lista todos os carrapatos cadastrados.
    /// </summary>
    /// <returns>Retorna 200 OK com a lista de carrapatos, 500 Internal Server Error ou 503 Service Unavailable em caso de erro.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IEnumerable<CarrapatoLeituraDto>>> GetTodos()
    {
        try
        {
            var carrapatos = await _servico.ObterTodos();
            return Ok(carrapatos);
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar carrapatos: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro interno: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar carrapatos");
        }
    }

    /// <summary>
    /// Busca um carrapato pelo id.
    /// </summary>
    /// <param name="id">Id do carrapato</param>
    /// <returns>Retorna 200 OK com o carrapato, 404 Not Found se não existir, 500 ou 503 em caso de erro.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CarrapatoLeituraDto>> GetPorId(int id)
    {
        try
        {
            var carrapato = await _servico.ObterPorId(id);
            return Ok(carrapato);
        }
        catch (ExcecaoEntidadeNaoEncontrada)
        {
            return NotFound();
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar carrapato: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro interno: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao buscar carrapato");
        }
    }

    /// <summary>
    /// Cria um novo carrapato.
    /// </summary>
    /// <param name="dto">Dados para criação do carrapato</param>
    /// <returns>Retorna 201 Created com o carrapato criado, 500 ou 503 em caso de erro.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CarrapatoLeituraDto>> Criar([FromBody] CarrapatoCriarDto dto)
    {
        try
        {
            var criado = await _servico.Criar(dto);
            return CreatedAtAction(nameof(GetPorId), new { id = criado.Id }, criado);
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao criar carrapato: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro interno: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao criar carrapato");
        }
    }

    /// <summary>
    /// Atualiza um carrapato existente.
    /// </summary>
    /// <param name="id">Id do carrapato</param>
    /// <param name="dto">Dados para atualização</param>
    /// <returns>Retorna 200 OK com o carrapato atualizado, 404 Not Found se não existir, 500 ou 503 em caso de erro.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<CarrapatoLeituraDto>> Atualizar(int id, [FromBody] CarrapatoCriarDto dto)
    {
        try
        {
            var atualizado = await _servico.Atualizar(id, dto);
            return Ok(atualizado);
        }
        catch (ExcecaoEntidadeNaoEncontrada)
        {
            return NotFound();
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao atualizar carrapato: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro interno: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao atualizar carrapato");
        }
    }

    /// <summary>
    /// Remove um carrapato pelo id.
    /// </summary>
    /// <param name="id">Id do carrapato</param>
    /// <returns>Retorna 204 No Content se removido, 404 Not Found se não existir, 500 ou 503 em caso de erro.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult> Remover(int id)
    {
        try
        {
            var removido = await _servico.Remover(id);
            if (!removido) return NotFound();
            return NoContent();
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao remover carrapato: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro interno: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno ao remover carrapato");
        }
    }
}
