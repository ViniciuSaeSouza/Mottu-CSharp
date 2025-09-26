namespace Aplicacao.DTOs.Moto;

public class MotoAtualizarDto
{
    public string? Placa { get; set; }
    public string? Modelo { get; set; }
    public int? IdFilial { get; set; }
    public int? IdCarrapato { get; set; }
    public MotoAtualizarDto() { }
}