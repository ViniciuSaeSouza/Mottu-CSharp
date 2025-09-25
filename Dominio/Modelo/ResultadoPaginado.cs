using Dominio.Interfaces;

namespace Dominio.Modelo;

public class ResultadoPaginado<T> : IResultadoPaginado<T>
{
    public bool TemProximo => Pagina < TotalPaginas;

    public bool TemAnterior => Pagina > 1;
    
    public IEnumerable<T> Items { get; set; } = new List<T>();
    
    public int Pagina { get; set; }
    
    public int TamanhoPagina { get; set; }
    
    public int ContagemTotal { get; set; }

    public int TotalPaginas => (int)Math.Ceiling(ContagemTotal / (double)TamanhoPagina);
}