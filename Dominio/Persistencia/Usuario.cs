using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Usuario
{
    public int Id { get; private set; }
    //public string FirebaseUid { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public string Senha { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public int IdPatio { get; private set; }
    public Patio Patio { get; private set; }

    // Para EF Core
    protected Usuario()
    {
    }
    
    public Usuario(string email, string nome, string senha)
    {
        ValidarUsuario(email, nome, senha);

        Email = email;
        Nome = nome;
        Senha = senha;
        DataCriacao = DateTime.UtcNow;
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
    
}
