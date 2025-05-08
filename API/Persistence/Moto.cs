namespace API.Persistence
{
    public class Moto
    {
        public Guid Id { get; set; }
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public string Status { get; set; }
        public Guid IdFilial { get; set; }
    }
}
