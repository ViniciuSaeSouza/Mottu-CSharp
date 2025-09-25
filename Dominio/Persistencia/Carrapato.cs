using Dominio.Enumeradores;

namespace Dominio.Persistencia;

public class Carrapato
{
    public int Id { get; private set; }
    public StatusBateriaEnum StatusBateria { get; set; }
    public int IdPatio { get; set; }
    public Patio Patio { get; set; }
    public int? IdMoto { get; set; }
    public Moto? Moto { get; set; }
    
    // Para EF Core
    protected Carrapato(){}

    public Carrapato(int? IdMoto, int IdPatio)
    {
        
    }
}