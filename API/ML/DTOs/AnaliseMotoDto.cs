namespace API.ML.DTOs;

public class AnaliseMotoDto
{
    public string Id { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public bool PrecisaManutencao { get; set; }
    public double Probabilidade { get; set; }
    public double Score { get; set; }
    public string AvaliacaoRisco { get; set; } = string.Empty;
    public int Prioridade { get; set; }
}
