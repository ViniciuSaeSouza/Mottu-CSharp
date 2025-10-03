
namespace Aplicacao.Abstracoes;

public class Recurso<T>
{
    public required T Dados { get; set; }
    public required List<Link> Links { get; set; }
}