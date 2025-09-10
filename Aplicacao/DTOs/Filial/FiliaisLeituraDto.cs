using Aplicacao.DTOs.Moto;

namespace Aplicacao.DTOs.Filial;

public class FiliaisLeituraDto
{
    public int Id { get; set; } // Identificador da filial
    public string Nome { get; set; } // Nome da filial
    public string Endereco { get; set; } // Endereço da filial

    // TODO: Deixar lista de motos opcional
    public ICollection<MotoLeituraDto> Motos { get; set; } = new List<MotoLeituraDto>();// Coleção de motos associadas à filial
    public FiliaisLeituraDto(int id, string nome, string endereco, ICollection<MotoLeituraDto> motos)
    {
        Id = id;
        Nome = nome;
        Endereco = endereco;
        Motos = motos;
    }

    public FiliaisLeituraDto() { }
}
