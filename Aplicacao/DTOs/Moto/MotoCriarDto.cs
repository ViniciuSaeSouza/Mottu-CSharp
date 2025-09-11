using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Moto;
public class MotoCriarDto
{
    // TODO: Remover validações do DTO e manter em DOMAIN
    [Required(ErrorMessage = "Placa não pode estar vazia.")]
    [StringLength(7, MinimumLength = 6, ErrorMessage = "Placa deve ter no mínmo 6 e no máximo 7 caracteres.")]
    public string Placa { get; set; }

    [Required(ErrorMessage = "Modelo não pode estar vazio.")]
    public string Modelo { get; set; }

    [Required(ErrorMessage = "Filial não pode estar vazia.")]
    public int IdFilial { get; set; }

    public MotoCriarDto(string placa, string modelo, int idFilial)
    {
        Placa = placa;
        Modelo = modelo;
        IdFilial = idFilial;
    }
}