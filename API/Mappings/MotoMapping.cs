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
                .ToTable("tbl_moto");

            builder.HasKey(m => m.Id);

            builder
                .Property(m => m.Placa)              
                .IsRequired();

            builder.HasIndex(m => m.Placa)
                .IsUnique();

            builder
                .HasOne(m => m.Modelo)
                .WithMany(md => md.Motos)
                .HasForeignKey(m => m.IdModelo)
                .IsRequired();

            builder
                .HasOne(m => m.Filial)
                .WithMany(f => f.Motos)
                .HasForeignKey(m => m.IdFilial)
                .IsRequired();
                
        }
    }
}
