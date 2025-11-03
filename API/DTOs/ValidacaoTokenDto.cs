namespace API.DTOs;

public class ValidacaoTokenDto
{
    public bool Valido { get; set; }
    public string Mensagem { get; set; } = string.Empty;
}
