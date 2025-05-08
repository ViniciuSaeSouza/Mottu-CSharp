namespace API.Persistence
{
    public class Sensor
    {
        public Guid Id { get; set; }
        public string CodigoSensor { get; set; }
        public Zona Zona { get; set; }
        public Guid IdZona { get; set; }

    }
}
