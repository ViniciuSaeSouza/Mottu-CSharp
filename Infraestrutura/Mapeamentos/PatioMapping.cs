using Dominio.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos;

public class PatioMapping : IEntityTypeConfiguration<Patio>
{
    public void Configure(EntityTypeBuilder<Patio> builder)
    {
        
        builder.ToTable("PATIO");
        
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(f => f.Nome)
            .IsUnique();

        builder.Property(f => f.Endereco)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasMany(f => f.Motos)
            .WithOne(m => m.Patio)
            .HasForeignKey(m => m.idPatio);

    }
}