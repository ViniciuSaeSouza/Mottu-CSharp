namespace API.ML.DTOs;

public class PredicaoAnaliseFrotaResumoDto
{
    public int TotalMotos { get; set; }
    public int MotosComManutencao { get; set; }
    public double PercentualManutencao { get; set; }
    public DateTime DataAnalise { get; set; }
    public List<AnaliseMotoDto> Ranking { get; set; } = new();
}
