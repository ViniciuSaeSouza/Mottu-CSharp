namespace API.Persistence
{
    public class ZonaPadrao
    {
        public Guid Id { get; private set; }

        public string NomeZona { get; set; }

        public string Descricao { get; set; }

        public ZonaPadrao(string nomeZona, string descricao)
        {
            this.NomeZona = nomeZona;
            this.Descricao = descricao;
        }
    }
}
