using System.ComponentModel.DataAnnotations;

namespace API.Persistence
{
    public class Moto
    {
        public Guid Id { get; set; }

        [StringLength(7, MinimumLength = 7)]
        public string Placa { get; set; }

        public Guid IdModelo { get; set; }
        public Modelo Modelo { get; set; }

        public Guid IdFilial { get; set; }
        public Filial Filial { get; set; } // TODO - alterar para HistoricoMotoFilial
    }
}
