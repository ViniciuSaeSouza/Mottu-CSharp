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
                .ToTable("Enderecos");

            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Logradouro)
                .IsRequired();

            builder
                .Property(p => p.Cep)
                .IsRequired();

            builder
                .Property(p => p.Complemento);

            builder
                .Property(p => p.Numero)
                .IsRequired();

            builder
                .Property(p => p.IdCidade)
                .IsRequired();

        }
    }
}
