using Dominio.Persistencia.Mottu;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestrutura.Mapeamentos.Mottu;

public class MotoMottuMapping : IEntityTypeConfiguration<MotoMottu>
{
    public void Configure(EntityTypeBuilder<MotoMottu> builder)
    {
        builder.ToTable("MOTOS_MOTTU");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("ID_MOTO");

        builder.Property(x => x.Chassi)
            .IsRequired()
            .HasColumnName("CHASSI")
            .HasMaxLength(17);

        builder.Property(x => x.Placa)
            .IsRequired()
            .HasColumnName("PLACA")
            .HasMaxLength(7);

        builder.Property(x => x.Modelo)
            .IsRequired()
            .HasColumnName("ID_MODELO");
    }
}