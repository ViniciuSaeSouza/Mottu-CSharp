using Dominio.Enumeradores;

namespace Aplicacao.DTOs.Moto;

public class MotoAtualizarDto
{
    public string? Placa { get; set; }
    public int? Modelo { get; set; }
    public int? IdPatio { get; set; }
    public int? IdCarrapato { get; set; }
    public int? Zona { get; set; }
    public MotoAtualizarDto() { }
}