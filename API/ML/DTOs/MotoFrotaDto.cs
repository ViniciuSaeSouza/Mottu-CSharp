namespace API.ML.DTOs;

public class MotoFrotaDto
{
    public string Id { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public float KmRodados { get; set; }
    public float TempoUso { get; set; }
    public float NumeroViagens { get; set; }
    public float IdadeVeiculo { get; set; }
}
