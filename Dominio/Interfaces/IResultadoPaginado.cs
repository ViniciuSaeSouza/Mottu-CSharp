namespace Dominio.Interfaces;

public interface IResultadoPaginado<T>
{
    bool TemProximo { get; }
    bool TemAnterior { get; }
    IEnumerable<T> Items { get; set; }
    int Pagina { get; set; }
    int TamanhoPagina { get; set; }
    int ContagemTotal { get; set; }
    int TotalPaginas { get; }
}