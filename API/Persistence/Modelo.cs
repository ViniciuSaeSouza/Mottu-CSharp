namespace API.Persistence
{
    public class Modelo
    {

        public Guid Id { get; set; }
        public string NomeModelo { get; set; } // Não pode ter o mesmo nome da classe

        public ICollection<Moto> Motos{ get; set; }
    }
}