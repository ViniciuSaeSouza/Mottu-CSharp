using Dominio.Enumeradores;
using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Moto
{
    public int Id { get; private set; }
    public string Placa { get; private set; }
    public string Chassi { get; private set; }
    public ModeloMoto Modelo { get; private set; }
    public int IdFilial { get; private set; }
    public Filial Filial { get; private set; }

    public Moto(string placa, string nomeModelo, int idFilial, string chassi, Filial filial)
    {
        ValidarNuloVazio(
            (nameof(placa), placa),
            (nameof(nomeModelo), nomeModelo),
            (nameof(chassi), chassi)
        );

        ValidarPlaca(placa);

        Placa = placa.ToUpper();
        DefinirModelo(nomeModelo);
        IdFilial = idFilial;
        Filial = filial;
    }

    public Moto()
    {
    }


    private void ValidarChassi(string chassi)
    {
        throw new NotImplementedException();
    }


    private void ValidarNuloVazio(params (string NomeCampo, object ValorCampo)[] campos)
    {
        foreach (var campo in campos)
        {
            if (campo.ValorCampo is string str && string.IsNullOrWhiteSpace(str))
            {
                throw new ExcecaoDominio($"{campo.NomeCampo} não pode ser nulo ou vazio.", campo.NomeCampo);
            }
        }
    }

    private void ValidarPlaca(string placa)
    {
        if (placa.Length < 6 || placa.Length > 7)
        {
            throw new ExcecaoDominio("Placa deve ter no mínimo 6 e no máximo 7 caracteres.", nameof(placa));
        }
    }

    private void ValidarModelo(string nomeModelo)
    {
        var modeloUpper = nomeModelo.ToUpper();

        if (!Enum.IsDefined(typeof(ModeloMoto), modeloUpper))
        {
            throw new ExcecaoDominio("Modelo inválido.", nameof(nomeModelo));
        }
    }

    private void DefinirModelo(string nomeModelo)
    {
        ValidarModelo(nomeModelo);
        Modelo = Enum.Parse<ModeloMoto>(nomeModelo.ToUpper(), ignoreCase: true);
    }

    public void AlterarPlaca(string novaPlaca)
    {
        ValidarNuloVazio((nameof(novaPlaca), novaPlaca));
        ValidarPlaca(novaPlaca);
        Placa = novaPlaca.ToUpper();
    }

    public void AlterarModelo(string novoModelo)
    {
        ValidarNuloVazio((nameof(novoModelo), novoModelo));
        DefinirModelo(novoModelo);
    }

    public void AlterarFilial(int novoIdFilial, Filial novaFilial)
    {
        IdFilial = novoIdFilial;
        Filial = novaFilial;
    }
}