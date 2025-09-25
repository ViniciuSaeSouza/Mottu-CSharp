using Dominio.Enumeradores;

namespace Dominio.Persistencia;

public class Carrapato
{
    public int Id { get; private set; }
    public string CodigoSerial { get; set; }
    public StatusBateriaEnum StatusBateria { get; set; }
    public StatusDeUsoEnum StatusDeUso { get; set; }
    public int IdPatio { get; set; }
    public Patio Patio { get; set; }
    public int? IdMoto { get; set; }
    public Moto? Moto { get; set; }

    // Para EF Core
    protected Carrapato()
    {
    }

    public Carrapato(string codigoSerial, int idPatio, int? idMoto)
    {
        CodigoSerial = codigoSerial;
        IdPatio = idPatio;
        IdMoto = idMoto;
        StatusBateria = StatusBateriaEnum.Alta;
        StatusDeUso = StatusDeUsoEnum.Disponivel;
    }

    // private void ValidarCriacao(string codigoSerial)
    // {
    //     ValidarCodigoSerial(codigoSerial);
    //     
    // }
}