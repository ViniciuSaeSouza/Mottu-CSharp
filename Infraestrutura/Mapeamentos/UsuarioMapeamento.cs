using Dominio.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos;

public class UsuarioMapeamento : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("USUARIOS");
        
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Senha)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.DataCriacao)
            .IsRequired();
        
        builder.HasOne(u => u.Patio)
            .WithMany(p => p.Usuarios)
            .HasForeignKey(u => u.IdPatio)
            .OnDelete(DeleteBehavior.Restrict);


    }
}