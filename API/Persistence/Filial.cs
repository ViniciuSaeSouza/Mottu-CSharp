namespace API.Persistence
{
    public class Filial
    {
        public Guid Id { get; private set; }
        public string NomeFilial { get; set; }

        public Guid IdEndereco { get; set; }
        public Endereco endereco { get; set; }

        public ICollection<ZonaFilial> Zonas { get; set; }
        public ICollection<Moto> Motos { get; set; }
    }
}
