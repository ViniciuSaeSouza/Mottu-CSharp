using Dominio.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos;

public class FilialMapping : IEntityTypeConfiguration<Filial>
{
    public void Configure(EntityTypeBuilder<Filial> builder)
    {
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
            .WithOne(m => m.Filial)
            .HasForeignKey(m => m.IdFilial);

    }
}