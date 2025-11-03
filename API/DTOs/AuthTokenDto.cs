namespace API.DTOs;

public class AuthTokenDto
{
    public string Token { get; set; } = string.Empty;
    public string TipoToken { get; set; } = "Bearer";
    public int ExpiracaoEm { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public DateTime DataGeracao { get; set; }
}
