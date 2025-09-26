namespace Dominio.Interfaces;

public interface IResultadoPaginado<T>
{
    bool TemProximo { get; }
    bool TemAnterior { get; }
    IEnumerable<T> Items { get; set; }
    int Pagina { get; set; }
    int TamanhoPagina { get; set; }
    int ContagemTotal { get; set; }
    int TotalPaginas => (int) Math.Ceiling(ContagemTotal / (double) TamanhoPagina);
    // int Janela  =>  Pagina <= 1 ? 0 : (Pagina - 1) * TamanhoPagina; 
}