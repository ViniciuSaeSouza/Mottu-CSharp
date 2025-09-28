using Aplicacao.DTOs.Moto;
using Aplicacao.Servicos;
using Dominio.Excecao;
using Dominio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[Route("api/motos")]
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
    /// Obtém uma lista paginada de todas as motos cadastradas em um pátio.
    /// </summary>
    /// <param name="pagina">Número da página a ser retornada (padrão: 1)</param>
    /// <param name="tamanhoPagina">Número de itens por página (padrão: 10)</param>
    /// <returns>
    /// Retorna uma lista paginada de objetos MotoLeituraDto representando as motos.
    /// Retorna 200 OK se as motos forem encontradas.
    /// Retorna 500 Internal Server Error se ocorrer um erro interno no servidor.
    /// Retorna 503 Service Unavailable se o serviço estiver temporariamente indisponível.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IResultadoPaginado<MotoLeituraDto>>> GetMotos([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
    {
        try
        {
            var motos = await _motoServico.ObterTodosPaginado(pagina, tamanhoPagina);
            return Ok(motos);
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar todas as motos no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao buscar motos: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao buscar motos: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
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
    public async Task<ActionResult<MotoLeituraDto>> GetMoto(int id)
    {
        try
        {
            var moto = await _motoServico.ObterPorId(id);
            return Ok(moto);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Moto de id {id} não encontrada: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = $"Nenhuma moto encontrada para o id {id}" });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao buscar moto de id {id} no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao buscar moto {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<MotoLeituraDto>> CriarMoto([FromBody] MotoCriarDto motoDto)
    {
        try
        {
            var moto = await _motoServico.Criar(motoDto);
            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro ao criar moto: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Entidade não encontrada ao criar moto: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao criar moto: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao criar moto: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Atualiza uma moto existente pelo ID.
    /// </summary>
    /// <param name="id">ID da moto a ser atualizada</param>
    /// <param name="dto">Dados para atualização da moto</param>
    /// <returns>
    /// Retorna 200 OK com a moto atualizada, 404 Not Found se não existir, 400 Bad Request se inválido, 500 ou 503 em caso de erro.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<MotoLeituraDto>> AtualizarMoto(int id, [FromBody] MotoAtualizarDto dto)
    {
        try
        {
            var moto = await _motoServico.Atualizar(id, dto);
            return Ok(moto);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Moto de id {id} não encontrada para atualização: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = $"Nenhuma moto encontrada para o id {id}" });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha ao atualizar moto de id {id} no banco de dados: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao atualizar moto: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha interna da aplicação ao atualizar moto {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
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
    public async Task<ActionResult<MotoLeituraDto>> PatchMoto(int id, [FromBody] MotoAtualizarDto motoUpdateDto)
    {
        try
        {
            var motoAtualizada = await _motoServico.Atualizar(id, motoUpdateDto);
            return Ok(motoAtualizada);
        }
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Moto ou filial de id {id} não encontrada para atualização: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoDominio ex)
        {
            Console.WriteLine($"Erro de validação ao atualizar moto {id}: {ex.Message}\n{ex.StackTrace}");
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao atualizar moto {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao atualizar moto {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
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
        catch (ExcecaoEntidadeNaoEncontrada ex)
        {
            Console.WriteLine($"Moto de id {id} não encontrada para remoção: {ex.Message}\n{ex.StackTrace}");
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ExcecaoBancoDados ex)
        {
            Console.WriteLine($"Falha no banco de dados ao remover moto {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de banco de dados indisponível");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro não tratado ao remover moto {id}: {ex.Message}\n{ex.StackTrace}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro interno do servidor");
        }
    }
}