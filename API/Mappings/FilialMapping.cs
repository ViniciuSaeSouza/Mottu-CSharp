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
                .ToTable("Filiais");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.NomeFilial)
                .IsRequired();

            builder
                .Property(p => p.endereco)
                .IsRequired();
        }
    }
}
