using Dominio.Enumeradores;

namespace Dominio.Persistencia.Mottu;

public record MotoMottu
{
    public int Id { get; init; }
    public string Placa { get; init; }
    public string Chassi { get; init; }
    public ModeloMotoEnum Modelo { get; init; }

    public MotoMottu(int id, string placa, string chassi, ModeloMotoEnum modeloMotoEnum)
    {
        Id = id;
        Placa = placa;
        Chassi = chassi;
        Modelo = modeloMotoEnum;
    }

    // Construtor sem parâmetros para Entity Framework
    public MotoMottu()
    {
        
    }
}