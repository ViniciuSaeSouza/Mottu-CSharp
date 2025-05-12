using System.ComponentModel.DataAnnotations;

namespace API.Persistence
{
    public class Sensor
    {
        [Key]
        public Guid Id { get; set; }
        public string CodigoSensor { get; set; }
        public Zona Zona { get; set; }
        public Guid IdZona { get; set; }

    }
}
