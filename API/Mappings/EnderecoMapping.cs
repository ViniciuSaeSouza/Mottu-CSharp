using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Mappings
{
    public class EnderecoMapping : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder
                .ToTable("tbl_endereco");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Logradouro)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(p => p.Cep)
                .IsRequired();

            builder
                .Property(p => p.Complemento)
                .HasMaxLength(30);

            builder
                .Property(p => p.Numero)
                .HasAnnotation("MinValue", 1)
                .IsRequired();

            builder
                .Property(p => p.Cidade)
                .HasMaxLength(50)
                .IsRequired();

        }
    }
}
