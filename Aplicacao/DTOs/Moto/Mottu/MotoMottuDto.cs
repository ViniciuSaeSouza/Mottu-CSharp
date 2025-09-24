using Dominio.Enumeradores;

namespace Aplicacao.DTOs.Moto.Mottu;

public record MotoMottuDto
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Chassi { get; set; }
    public string NomeModelo { get; set; }

    public MotoMottuDto(int id,string placa, string chassi, ModeloMotoEnum modeloMotoEnum)
    {
        Id = id;
        Placa = placa;
        Chassi = chassi;
        NomeModelo = modeloMotoEnum.ToString();
    }
}