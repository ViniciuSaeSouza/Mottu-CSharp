using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Filial;

public class FilialCriarDto
{
    public string Nome { get; set; }
    public string Endereco { get; set; }

    public FilialCriarDto(string nome, string endereco)
    {
        Nome = nome;
        Endereco = endereco;
    }
}