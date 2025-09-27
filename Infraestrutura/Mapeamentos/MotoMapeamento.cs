using Dominio.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos;

public class MotoMapeamento : IEntityTypeConfiguration<Moto>
{
    public void Configure(EntityTypeBuilder<Moto> builder)
    {
        builder.ToTable("MOTOS");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Placa)
            .IsRequired()
            .HasMaxLength(7);

        builder.HasIndex(m => m.Placa)
            .IsUnique();

        builder.Property(m => m.Chassi)
            .IsRequired()
            .HasMaxLength(17);

        builder.HasIndex(m => m.Chassi)
            .IsUnique();
        
        builder.Property(m => m.Modelo)
            .IsRequired();

        builder.Property(m => m.Zona)
            .IsRequired();

        builder.HasOne(m => m.Patio)
            .WithMany(f => f.Motos)
            .HasForeignKey(m => m.IdPatio)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Carrapato)
            .WithOne()
            .HasForeignKey<Moto>(m => m.IdCarrapato)
            .OnDelete(DeleteBehavior.NoAction);
    }
}