using System.ComponentModel.DataAnnotations;

namespace Aplicacao.DTOs.Moto;
public class MotoCriarDto
{
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public int IdFilial { get; set; }

    public MotoCriarDto(string placa, string modelo, int idFilial)
    {
        Placa = placa;
        Modelo = modelo;
        IdFilial = idFilial;
    }
}