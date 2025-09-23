using Dominio.Excecao;

namespace Dominio.Persistencia;

public class Usuario
{
    public int Id { get; private set; }
    public string FirebaseUid { get; private set; }
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public int? IdFilial { get; private set; }
    public Patio Patio { get; private set; }

    // Para EF Core
    protected Usuario()
    {
    }
    
    public Usuario(string firebaseUid, string email, string nome)
    {
        ValidarUsuario(firebaseUid, email, nome);
        FirebaseUid = firebaseUid;
        Email = email;
        Nome = nome;
        DataCriacao = DateTime.UtcNow;
    }

    private void ValidarUsuario(string firebaseUid, string email, string nome)
    {
        // Validando cada campo individualmente para mensagens de erro mais específicas
        if (string.IsNullOrEmpty(firebaseUid))
            throw new ExcecaoDominio("FirebaseUid não pode ser vazio ou nulo", nameof(firebaseUid));
            
        if (string.IsNullOrEmpty(email))
            throw new ExcecaoDominio("Email não pode ser vazio ou nulo", nameof(email));
            
        if (string.IsNullOrEmpty(nome))
            throw new ExcecaoDominio("Nome não pode ser vazio ou nulo", nameof(nome));
    }

    
    // Método para associar o usuário a uma filial
    public void AssociarFilial(int idFilial, Patio patio)
    {
        if (patio == null)
            throw new ExcecaoDominio("Filial não pode ser nula", nameof(patio));
            
        IdFilial = idFilial;
        Patio = patio;
    }
    
    
    public void DesassociarFilial()
    {
        IdFilial = null;
        Patio = null;
    }
}
