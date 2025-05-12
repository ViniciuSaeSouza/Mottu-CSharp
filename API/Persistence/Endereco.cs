using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Persistence
{
    public class Endereco
    {
        [Key]
        public Guid Id { get; private set; }
        [Required]
        public string Logradouro { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 8)]
        public string Cep { get; set; }

        public string Complemento { get; set; }
        [Required]
        public int Numero { get; set; }

        [ForeignKey("IdCidade")]
        public int IdCidade { get; set; }

        public Endereco(string logradouro, string cep, string complemento, int numero, int idCidade)
        {
            this.Logradouro = logradouro;
            this.Cep = cep;
            this.Complemento = complemento ?? "";
            this.Numero = numero;
            this.IdCidade = idCidade;
        }
    }
}