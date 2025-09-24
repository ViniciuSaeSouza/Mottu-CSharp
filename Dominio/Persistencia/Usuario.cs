using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get;  set; }
    public string Nome { get; set; }
    public string Senha { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public int IdPatio { get; set; }
    public Patio Patio { get; set; }

    // Para EF Core
    protected Usuario()
    {
    }
    
    public Usuario(string nome, string email, string senha, int idPatio)
    {
        ValidarUsuario(nome, email, senha);

        Email = nome;
        Nome = email;
        Senha = senha;
        DataCriacao = DateTime.UtcNow;
        IdPatio = idPatio;
    }

    private void ValidarUsuario( string email, string nome, string senha)
    {
        if (string.IsNullOrEmpty(email))
            throw new ExcecaoDominio("Email não pode ser vazio ou nulo", nameof(email));
            
        if (string.IsNullOrEmpty(nome))
            throw new ExcecaoDominio("Nome não pode ser vazio ou nulo", nameof(nome));
        
        if (string.IsNullOrEmpty(senha))
            throw new ExcecaoDominio("Senha não pode ser vazio ou nulo", nameof(senha));
    }
    
    public void AssociarPatioAoUsuario(int idPatio, Patio patio)
    {
        if (patio == null)
            throw new ExcecaoDominio("Pátio não pode ser nulo", nameof(patio));
            
        IdPatio = idPatio;
        Patio = patio;
    }
    
    public void AlterarNome(string nome)
    {
        if (string.IsNullOrEmpty(nome))
            throw new ExcecaoDominio("Nome não pode ser vazio ou nulo", nameof(nome));
            
        Nome = nome;
    }
    
    public void AlterarEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ExcecaoDominio("Email não pode ser vazio ou nulo", nameof(email));
            
        Email = email;
    }
    
    public void AlterarSenha(string senha)
    {
        if (string.IsNullOrEmpty(senha))
            throw new ExcecaoDominio("Senha não pode ser vazia ou nula", nameof(senha));
            
        Senha = senha;
    }
    
    public void AlterarPatio(int idPatio)
    {
        if (idPatio <= 0)
            throw new ExcecaoDominio("Id do pátio deve ser maior que zero", nameof(idPatio));
        IdPatio = idPatio;
    }
}
