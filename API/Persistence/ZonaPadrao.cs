namespace API.Persistence
{
    public class ZonaPadrao
    {
        public Guid Id { get; private set; }

        public string NomeZona { get; set; }

        public string Descricao { get; set; }

        public string CorZona { get; set; }

        public ICollection<ZonaFilial> Zonas { get; set; }

        public ZonaPadrao(string nomeZona, string descricao, string corZona)
        {
            this.NomeZona = nomeZona;
            this.Descricao = descricao;
            this.CorZona = corZona;
        }
    }
}
