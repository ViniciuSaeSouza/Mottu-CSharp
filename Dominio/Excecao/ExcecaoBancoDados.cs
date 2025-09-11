namespace Dominio.Excecao;

public class ExcecaoBancoDados : Exception
{
    public ExcecaoBancoDados(string message, string item) : base(message)
    {
    }
}
