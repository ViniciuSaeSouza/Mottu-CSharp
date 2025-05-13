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

            builder.HasOne(p => p.Zona)
                .WithOne(z => z.Sensor)
                .HasForeignKey("IdZona");
        }
    }
}
