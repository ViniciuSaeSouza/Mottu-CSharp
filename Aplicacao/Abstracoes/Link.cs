namespace Aplicacao.Abstracoes;

public class Link
{
    public required string Rel { get; set; } // ex: "self", "update", "delete", "next"
    public required string Href { get; set; } // url completa
    public required string Method { get; set; } // GET, POST, PUT, PATCH, DELETE
}