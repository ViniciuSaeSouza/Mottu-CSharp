namespace Dominio.Excecoes;

public class ExcecaoDominio : Exception
{
    public ExcecaoDominio(string message, string item) : base(message)
    {
    }
}
