using Dominio.Excecoes;

namespace Dominio.Persistencia;

public class Filial
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Endereco { get; private set; }
    public ICollection<Moto> Motos { get; private set; } = new List<Moto>();

    public Filial(string nome, string endereco)
    {
        DefinirNome(nome);
        DefinirEndereco(endereco);

        // preparar o método pra receber uma lista de atributos (array com nome e endereço) nome deu false, interpola com o atributo nome
    }

    private void DefinirEndereco(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
        {
            throw new ExcecaoDominio("Endereço não pode ser nulo ou vazio.", nameof(endereco));
        }

        this.Endereco = endereco;
    }

    private void DefinirNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(nome));
        }
        this.Nome = nome;
    }

    public void AlterarEndereco(string novoEndereco)
    {
        DefinirEndereco(novoEndereco);
    }

    public void AlterarNome(string novoNome)
    {
        DefinirNome(novoNome);
    }

}