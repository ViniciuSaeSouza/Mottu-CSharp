namespace Aplicacao.DTOs.Patio;

public class PatioAtualizarDto
{
    public string? Nome { get; set; }
    public string? Endereco { get; set; }
    public PatioAtualizarDto(string nome, string endereco)
    {
        Nome = nome;
        Endereco = endereco;
    }

    public PatioAtualizarDto() { }
}