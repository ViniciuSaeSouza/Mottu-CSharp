namespace API.ML.DTOs;

public class PredicaoResultadoDto
{
    public bool PrecisaManutencao { get; set; }
    public double Probabilidade { get; set; }
    public double Score { get; set; }
    public string AvaliacaoRisco { get; set; } = string.Empty;
    public string Recomendacao { get; set; } = string.Empty;
    public DateTime DataAnalise { get; set; }
}
