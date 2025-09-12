using Dominio.Excecao;

namespace Aplicacao.Validacoes
{
    public class ValidacaoEntidade
    {
        public static void LancarSeNulo<T>(T entidade, string nomeEntidade, object id)
        {
            if (entidade == null)
                throw new ExcecaoEntidadeNaoEncontrada(nomeEntidade, id);
        }
        public static void AlterarValor(string valor, Action<string> alterar)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                alterar(valor);
            }
        }

        public static void ValidarValor(string valor, Action<string> validar = null)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                validar?.Invoke(valor);
            }
        }


    }
}
