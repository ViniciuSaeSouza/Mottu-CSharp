using Dominio.Excecao;
using Dominio.Interfaces;
using Dominio.Persistencia;
using Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorios;

public class UsuarioRepositorio : IRepositorioUsuario
{
    private readonly AppDbContext _contexto;

    public UsuarioRepositorio(AppDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<List<Usuario>> ObterTodos()
    {
        try
        {
            return await _contexto.Usuarios.Include(u => u.Patio).ToListAsync();
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter usuários do banco de dados",
                nameof(Usuario), ex);
        }
    }

    public Task<IResultadoPaginado<Usuario>> ObterTodosPaginado(int pagina, int totalPaginas)
    {
        throw new NotImplementedException();
    }

    public async Task<Usuario?> ObterPorId(int id)
    {
        try
        {
            return await _contexto.Usuarios.Include(u => u.Patio)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter usuário do banco de dados", nameof(Usuario),
                ex);
        }
    }
    
    public async Task<Usuario?> ObterPorEmail(string email)
    {
        try
        {
            return await _contexto.Usuarios.Include(u => u.Patio)
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao obter usuário do banco de dados", nameof(Usuario),
                innerException: ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao obter usuário do banco de dados", nameof(Usuario),
                ex);
        }
    }

    public async Task<Usuario> Adicionar(Usuario usuario)
    {
        try
        {
            _contexto.Usuarios.Add(usuario);
            await _contexto.SaveChangesAsync();
            return usuario;
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao adicionar usuário no banco de dados", nameof(Usuario),
                innerException: ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao adicionar usuário no banco de dados",
                nameof(Usuario), ex);
        }
    }

    public async Task<Usuario> Atualizar(Usuario usuario)
    {
        try
        {
            _contexto.Usuarios.Update(usuario);
            await _contexto.SaveChangesAsync();
            return usuario;
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao atualizar usuário no banco de dados", nameof(Usuario),
                innerException: ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao atualizar usuário no banco de dados",
                nameof(Usuario), ex);
        }
    }

    public async Task<bool> Remover(Usuario usuario)
    {
        try
        {
            _contexto.Usuarios.Remove(usuario);
            await _contexto.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            throw new ExcecaoBancoDados("Falha ao remover usuário do banco de dados", nameof(Usuario),
                innerException: ex);
        }
        catch (OperationCanceledException ex)
        {
            throw new ExcecaoBancoDados("Falha, operação cancelada ao remover usuário do banco de dados",
                nameof(Usuario), ex);
        }
    }
    
}