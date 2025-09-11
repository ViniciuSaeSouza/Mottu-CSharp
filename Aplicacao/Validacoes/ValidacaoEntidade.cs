using Dominio.Excecao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Validacoes
{
    public class ValidacaoEntidade
    {
        public static void LancarSeNulo<T>(T entidade, string nomeEntidade, object id)
        {
            if (entidade == null)
                throw new EntidadeNaoEncontradaException(nomeEntidade, id);
        }
    }
}
