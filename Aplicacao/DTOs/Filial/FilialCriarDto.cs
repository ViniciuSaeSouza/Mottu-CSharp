using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Filial;

public class FilialCriarDto
{
    // TODO: Remover validação daqui e manter na classe de DOMAIN

    [Required(ErrorMessage = "Nome da filial não pode estar vazio")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "Endereço da filial não pode estar vazio")]
    public string Endereco { get; set; }

    public FilialCriarDto(string nome, string endereco)
    {
        Nome = nome;
        Endereco = endereco;
    }
}
