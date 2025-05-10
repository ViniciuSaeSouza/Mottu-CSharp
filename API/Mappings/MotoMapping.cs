using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder
                .ToTable("Motos");

            builder.HasKey("Id");

            builder
                .Property(p => p.Placa)
                .IsRequired();

            builder
                .Property(p => p.Modelo)
                .IsRequired();

            builder
                .Property(p => p.Status)
                .IsRequired();

            builder
                .Property(p => p.IdFilial)
                .IsRequired();
        }
    }
}
