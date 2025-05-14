namespace API.Persistence
{
    public class ZonaFilial
    {
        public Guid Id { get; set; }

        public Guid IdFilial { get; set; }
        public Filial Filial { get; set; }

        public Guid IdZonaPadrao { get; set; }
        public ZonaPadrao ZonaPadrao { get; set; }

        public Sensor Sensor { get; set; }

        public ICollection<LeituraSinal> Leituras { get; set; }
    }
}