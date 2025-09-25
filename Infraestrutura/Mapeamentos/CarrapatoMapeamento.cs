using Dominio.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos;

public class CarrapatoMapeamento : IEntityTypeConfiguration<Carrapato>
{
    public void Configure(EntityTypeBuilder<Carrapato> builder)
    {
        builder.ToTable("CARRAPATOS");
        
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CodigoSerial)
            .IsRequired()
            .HasMaxLength(7);

        builder.HasIndex(c => c.CodigoSerial)
            .IsUnique();

        builder.Property(c => c.StatusBateria)
            .IsRequired();

        builder.Property(c => c.StatusDeUso)
            .IsRequired();

        builder.HasOne(c => c.Patio)
            .WithMany(p => p.Carrapatos)
            .HasForeignKey(c => c.IdPatio);

        builder.HasOne(c => c.Moto)
            .WithOne(m => m.Carrapato)
            .OnDelete(DeleteBehavior.NoAction);
    }
}