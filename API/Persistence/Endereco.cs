using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Persistence
{
    public class Endereco
    {

        public Guid Id { get; private set; }

        public string Logradouro { get; set; }

        public string Cep { get; set; }

        public string Complemento { get; set; }

        public int Numero { get; set; }

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