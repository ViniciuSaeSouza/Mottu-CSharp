using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Usuario
{
    public int Id { get; private set; }
    //public string FirebaseUid { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public string Password { get; set; }
    public DateTime DataCriacao { get; private set; }
    public int IdPatio { get; private set; }
    public Patio Patio { get; private set; }

    // Para EF Core
    protected Usuario()
    {
    }
    
    public Usuario(string email, string nome)
    {
        ValidarUsuario(email, nome);
        Email = email;
        Nome = nome;
        DataCriacao = DateTime.UtcNow;
    }

    private void ValidarUsuario( string email, string nome)
    {
        if (string.IsNullOrEmpty(email))
            throw new ExcecaoDominio("Email não pode ser vazio ou nulo", nameof(email));
            
        if (string.IsNullOrEmpty(nome))
            throw new ExcecaoDominio("Nome não pode ser vazio ou nulo", nameof(nome));
    }

    
    // Método para associar o usuário a um pátio
    public void AssociarPatio(int idPatio, Patio patio)
    {
        if (patio == null)
            throw new ExcecaoDominio("Patio não pode ser nulo", nameof(patio));
            
        IdPatio = idPatio;
        Patio = patio;
    }
    
}
