using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Persistence
{
    public class Endereco
    {

        public Guid Id { get; private set; }
        public string Logradouro { get; set; }

        [MinLength(8, ErrorMessage = "Cep deve conter 8 digitos.")]
        public string Cep { get; set; }

        public string Complemento { get; set; }

        [Range(1, int.MaxValue)] // int.MaxValue representa o valor máximo que um int pode assumir
        public int Numero { get; set; }

        public string Cidade { get; set; }


        public Endereco(string logradouro, string cep, string complemento, int numero, string Cidade)
        {
            this.Logradouro = logradouro;
            this.Cep = cep;
            this.Complemento = complemento ?? "";
            this.Numero = numero;
            this.Cidade = Cidade;
        }
    }
}