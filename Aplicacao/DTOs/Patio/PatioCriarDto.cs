using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Patio;

public class PatioCriarDto
{
    public string  Nome { get; set; }
    public string Endereco { get; set; }

    public PatioCriarDto(string nome, string endereco)
    {
        Nome = nome;
        Endereco = endereco;
    }
}