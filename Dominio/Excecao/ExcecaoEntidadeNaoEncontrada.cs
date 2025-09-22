
namespace Dominio.Excecao
{
    public class ExcecaoEntidadeNaoEncontrada(string entidade, object id) : Exception($"{entidade} com ID {id} não foi encontrada.")
    {
    }
}
