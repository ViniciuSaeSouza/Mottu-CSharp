using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class ZonaPadraoMapping : IEntityTypeConfiguration<ZonaPadrao>
    {
        public void Configure(EntityTypeBuilder<ZonaPadrao> builder)
        {
            builder
                .ToTable("tbl_zona_padrao");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.NomeZona)
                .HasMaxLength(30)
                .IsRequired();

            builder
                .Property(p => p.Descricao)
                .HasMaxLength(100)
                .IsRequired();
            
            builder
                .Property(p => p.CorZona)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .HasMany(zp => zp.Zonas)
                .WithOne(z => z.ZonaPadrao)
                .HasForeignKey(z => z.IdZonaPadrao);

        }
    }
}
