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
                .ToTable("tbl_sensor");

            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.NomeSensor)
                .HasMaxLength(11)
                .IsRequired();

            builder
                .HasIndex(s => s.NomeSensor)
                .IsUnique();

            builder.HasOne(s => s.Zona)
                .WithOne(z => z.Sensor)
                .HasForeignKey<Sensor>(s => s.IdZona)
                .IsRequired(); // Relação 1:1 exige especificar qual entidade (Quem tem a FK?)

            builder
                .HasIndex(s => s.IdZona)
                .IsUnique();
        }
    }
}
