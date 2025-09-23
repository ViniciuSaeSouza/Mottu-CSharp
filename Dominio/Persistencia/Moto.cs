using Dominio.Enumeradores;
using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Moto
{
    public int Id { get; private set; }
    public string Placa { get; private set; }
    public string Chassi { get; private set; }
    public ModeloMotoEnum Modelo { get; private set; }
    public ZonaEnum Zona { get; set; }
    public int idPatio { get; set; }
    public Patio Patio { get; set; }

    public Moto(string placa, string nomeModelo, int idPatio, string chassi, Patio patio)
    {
        ValidarNuloVazio(
            (nameof(placa), placa),
            (nameof(nomeModelo), nomeModelo),
            (nameof(chassi), chassi)
        );
        ValidarPlaca(placa);
        ValidarChassi(chassi);

        Placa = placa.ToUpper();
        DefinirModelo(nomeModelo);
        
        this.idPatio = idPatio;
        Patio = patio;
        Zona = ZonaEnum.Saguao;
        Chassi = chassi.ToUpper();
    }

    public Moto()
    {
    }


    private void ValidarChassi(string chassi)
    {
        if (chassi.Length != 17 )
            throw new ExcecaoDominio("Chassi deve ter 17 caracteres (VIN)", chassi);
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

        if (!Enum.IsDefined(typeof(ModeloMotoEnum), modeloUpper))
        {
            throw new ExcecaoDominio("Modelo inválido.", nameof(nomeModelo));
        }
    }

    private void DefinirModelo(string nomeModelo)
    {
        ValidarModelo(nomeModelo);
        Modelo = Enum.Parse<ModeloMotoEnum>(nomeModelo.ToUpper(), ignoreCase: true);
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

    public void AlterarFilial(int novoIdFilial, Patio novoPatio)
    {
        idPatio = novoIdFilial;
        Patio = novoPatio;
    }

    public void DefinirZona(ZonaEnum zona)
    {
        Zona = zona;
    }
}