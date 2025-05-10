using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class SensorMapping : IEntityTypeConfiguration<Sensor>
    {
        public void Configure(EntityTypeBuilder<Sensor> builder)
        {
            builder
                .ToTable("Sensores");

            builder
                .HasKey(p => p.Id);

            builder
                .Property(p => p.CodigoSensor)
                .IsRequired();

            builder
                .Property(p => p.Zona)
                .IsRequired();

            builder
                .Property(p => p.IdZona)
                .IsRequired();
        }
    }
}
