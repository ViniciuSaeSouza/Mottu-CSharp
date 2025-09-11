
namespace Dominio.Excecao
{
    public class EntidadeNaoEncontradaException(string entidade, object id) : Exception($"{entidade} com ID {id} não foi encontrada.")
    {
    }
}
