using System.ComponentModel.DataAnnotations;

namespace API.Persistence
{
    public class Sensor
    {
        [Key]
        public Guid Id { get; set; }
        public string CodigoSensor { get; set; }
        public Guid IdZona { get; set; } // Tipo do id deve ser igual ao da entidade estrangeira
        public Zona Zona { get; set; }

    }
}
