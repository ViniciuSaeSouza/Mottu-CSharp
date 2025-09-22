using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Filial
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Endereco { get; private set; }
    public ICollection<Moto> Motos { get; private set; } = new List<Moto>();

    public Filial(string nome, string endereco)
    {
        ValidarNuloVazio(
            (nameof(nome), nome),
            (nameof(endereco), endereco)
        );

        Nome = nome;
        Endereco = endereco;
    }

    private void ValidarNuloVazio(params (string NomeCampo, string ValorCampo)[] campos)
    {
        foreach (var campo in campos)
        {
            if (string.IsNullOrWhiteSpace(campo.ValorCampo))
            {
                throw new ExcecaoDominio($"{campo.NomeCampo} não pode ser nulo ou vazio.", campo.NomeCampo);
            }
        }
    }

    public void AlterarEndereco(string novoEndereco)
    {
        ValidarNuloVazio((nameof(Endereco), novoEndereco));
        Endereco = novoEndereco;
    }

    public void AlterarNome(string novoNome)
    {
        ValidarNuloVazio((nameof(novoNome), novoNome));
        Nome = novoNome;
    }

}