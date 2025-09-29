using Dominio.Enumeradores;
using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Moto
{
    public int Id { get; private set; }
    public string Placa { get; private set; }
    public string Chassi { get; private set; }
    public ModeloMotoEnum Modelo { get; set; }
    public ZonaEnum Zona { get; set; }
    public int IdPatio { get; set; }
    public Patio Patio { get; set; }
    public int IdCarrapato { get; set; }
    public Carrapato Carrapato { get; set; }
    
    // Para EF Core
    public Moto()
    {
    }
    
    public Moto(string placa, ModeloMotoEnum modelo, int idPatio, string chassi, Patio patio, int idCarrapato)
    {
        ValidarNuloVazio(
            (nameof(placa), placa),
            (nameof(chassi), chassi)
        );
        ValidarPlaca(placa);
        ValidarChassi(chassi);
        Placa = placa.ToUpper();
        DefinirModelo((int)modelo);
        this.IdPatio = idPatio;
        Patio = patio;
        Zona = ZonaEnum.Saguao;
        Chassi = chassi.ToUpper();
        IdCarrapato = idCarrapato;
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

    private void ValidarModelo(int? modelo)
    {
        if (!Enum.IsDefined(typeof(ModeloMotoEnum), modelo))
        {
            throw new ExcecaoDominio("Modelo inválido.", nameof(modelo));
        }
    }

    private void DefinirModelo(int modelo)
    {
        ValidarModelo(modelo);
        Modelo = (ModeloMotoEnum)Enum.Parse(typeof(ModeloMotoEnum), modelo.ToString());
    }

    public void AlterarPlaca(string novaPlaca)
    {
        ValidarNuloVazio((nameof(novaPlaca), novaPlaca));
        ValidarPlaca(novaPlaca);
        Placa = novaPlaca.ToUpper();
    }

    public void AlterarModelo(int? modelo)
    {
        ValidarNuloVazio((nameof(modelo), modelo));
        DefinirModelo((int)modelo!);
    }

    public void AlterarFilial(int novoIdFilial, Patio novoPatio)
    {
        IdPatio = novoIdFilial;
        Patio = novoPatio;
    }
    
    public void AlterarZona(int zona)
    {
        if (!Enum.IsDefined(typeof(ZonaEnum), zona))
        {
            throw new ExcecaoDominio("Zona inválida.", nameof(zona));
        }
        Zona = (ZonaEnum)zona;
    }

    public void DefinirZona(ZonaEnum zona)
    {
        Zona = zona;
    }
}