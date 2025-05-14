using System.ComponentModel.DataAnnotations;

namespace API.Persistence
{
    public class Sensor
    {
        
        public Guid Id { get; set; }
        public string NomeSensor { get; set; }
        public Guid IdZona { get; set; } // Tipo do id deve ser igual ao da entidade estrangeira
        public ZonaFilial Zona { get; set; } // objeto de navegação 

    }
}
