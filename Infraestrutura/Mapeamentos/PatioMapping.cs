using Dominio.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos;

public class PatioMapping : IEntityTypeConfiguration<Patio>
{
    public void Configure(EntityTypeBuilder<Patio> builder)
    {
        
        builder.ToTable("PATIOS");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.Nome)
            .IsUnique();

        builder.Property(p => p.Endereco)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(p => p.Motos)
            .WithOne(m => m.Patio)
            .HasForeignKey(m => m.idPatio);
        
        builder.HasMany(p => p.Usuarios)
            .WithOne(u => u.Patio)
            .HasForeignKey(u => u.IdPatio);

    }
}