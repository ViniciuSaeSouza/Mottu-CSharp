namespace API.Persistence
{
    public class Zona
    {
        public Guid Id { get; set; }
        public Guid IdFilial { get; set; }
        public Guid IdZonaPadrao { get; set; }
        public ICollection<Sensor> Sensores { get; set; }
    }
}