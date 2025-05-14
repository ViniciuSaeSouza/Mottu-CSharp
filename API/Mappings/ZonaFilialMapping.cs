using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class ZonaFilialMapping : IEntityTypeConfiguration<ZonaFilial>
    {
        public void Configure(EntityTypeBuilder<ZonaFilial> builder)
        {
            builder
                .ToTable("tbl_zona_filial");

            builder
                .HasKey(p => p.Id);

            builder
                .HasOne(f => f.Filial)
                .WithMany(z => z.Zonas)
                .HasForeignKey(z => z.IdFilial);

            builder
                .HasOne(z => z.ZonaPadrao)
                .WithMany(zp => zp.Zonas)
                .HasForeignKey(z => z.IdZonaPadrao)
                .IsRequired();
        }
    }
}
