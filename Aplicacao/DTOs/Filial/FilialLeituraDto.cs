
using Aplicacao.DTOs.Moto;

namespace Aplicacao.DTOs.Filial;

public class FilialLeituraDto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Endereco { get; set; }

    // TODO: Deixar lista de motos opcional
    public ICollection<MotoLeituraDto> Motos { get; set; } = new List<MotoLeituraDto>();
    public FilialLeituraDto(int id, string nome, string endereco, ICollection<MotoLeituraDto> motos)
    {
        Id = id;
        Nome = nome;
        Endereco = endereco;
        Motos = motos;
    }

    public FilialLeituraDto() { }
}