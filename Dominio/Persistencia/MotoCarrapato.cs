using Dominio.Excecao;

namespace Dominio.Persistencia;

public class MotoCarrapato
{
    public int IdMoto { get; set; }
    public int IdPatio { get; set; }
    public int IdCarrapato { get; set; }
    public DateTime DataDeVinculamento { get; set; }
    public DateTime DataDeDesvinculamento { get; set; }
    public Moto Moto { get; set; }
    public Patio Patio { get; set; }
    public Carrapato Carrapato { get; set; }

    public MotoCarrapato()
    {
    }

    public MotoCarrapato(int idMoto, int idPatio, int idCarrapato)
    {
        var listaIds = new[] { idMoto, idPatio, idCarrapato };
        var nomesPropriedades = new[] { nameof(IdMoto), nameof(IdPatio), nameof(IdCarrapato) };
        ValidarIds(listaIds, nomesPropriedades);
        
        IdMoto = idMoto;
        IdPatio = idPatio;
        IdCarrapato = idCarrapato;
        DataDeVinculamento = DateTime.Now;
    }

    private void ValidarIds(int[] ids, string[] nomesPropriedades)
    {
        for (var i = 0; i < ids.Length; i++)
        {
            var id = ids[i];
            var nomePropriedade = nomesPropriedades[i];
            
            if (id <= 0)
                throw new ExcecaoDominio($"Id {nomePropriedade.ToUpper()} inválido: {id}", nameof(id));
        }
    }
    
    
    public void DesvincularCarrapato(int idCarrapato)
    {
        ValidarIds([idCarrapato], [nameof(IdCarrapato)]);
        DataDeDesvinculamento = DateTime.Now;
    }
}