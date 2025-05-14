using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class FilialMapping : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder
                .ToTable("tbl_filial");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.NomeFilial)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .HasIndex(f => f.NomeFilial)
                .IsUnique();

            builder
                 .HasMany(f => f.Motos)
                 .WithOne(m => m.Filial)
                 .HasForeignKey(m => m.IdFilial);

            builder
                .HasMany(f => f.Zonas)
                .WithOne(z => z.Filial)
                .HasForeignKey(z => z.IdFilial);
        }
    }
}
