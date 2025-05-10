using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class ZonaMapping : IEntityTypeConfiguration<Zona>
    {
        public void Configure(EntityTypeBuilder<Zona> builder)
        {
            builder
                .ToTable("Zonas");

            builder
                .HasKey(p =>  p.Id);

            builder
                .Property(p => p.IdFilial);

            builder
                .Property(p => p.IdZonaPadrao);
        }
    }
}
