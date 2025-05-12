namespace API.Persistence
{
    public class Filial
    {
        public Guid Id { get; private set; }

        public string NomeFilial { get; set; }

        public int IdEndereco { get; set; }

        public Endereco endereco { get; set; }
    }
}
