using Dominio.Enumeradores;
using Dominio.Excecoes;

namespace Dominio.Persistencia;

public class Moto
{
    public int Id { get; private set; }
    public string Placa { get; private set; }
    public ModeloMoto Modelo { get; private set; }
    public int IdFilial { get; private set; }
    public Filial Filial { get; private set; }

    public Moto(string placa, string nomeModelo, int idFilial, Filial filial)
    {
        DefinirPlaca(placa);
        DefinirModelo(nomeModelo);
        IdFilial = idFilial;
        Filial = filial;
    }

    public Moto() { }

    private void Validar() {
        // aqui eu passo o validar null or empty da lista
        // aqui eu passo o validar placa
    }

    private void ValidarPlaca(string placa)
    {

    // método que vai chamar o método verificação lenght
        if (string.IsNullOrWhiteSpace(placa))
        {
            throw new ExcecaoDominio("Placa não pode ser nula ou vazia.", nameof(placa));
        }
        else if (placa.Length < 6 || placa.Length > 7)
        {
            throw new ArgumentException("Placa deve ter no mínimo 6 e no máximo 7 caracteres.", nameof(placa));
        }

        this.Placa = placa.ToUpper();
    }
    private void DefinirModelo(string nomeModelo)
    {
        var modeloUpper = nomeModelo.ToUpper();
        if (!Enum.IsDefined(typeof(ModeloMoto), modeloUpper))
        {
            throw new ArgumentOutOfRangeException(nameof(nomeModelo), "Modelo inválido.");
        }
        Modelo = Enum.Parse<ModeloMoto>(modeloUpper, ignoreCase: true);
    }

    public void AlterarPlaca(string novaPlaca)
    {
        DefinirPlaca(novaPlaca);
    }

    public void AlterarModelo(string novoModelo)
    {
        DefinirModelo(novoModelo);
    }

    public void AlterarFilial(int novoIdFilial, Filial novaFilial)
    {
        IdFilial = novoIdFilial;
        Filial = novaFilial;
    }
}