using Dominio.Persistencia;
using Infraestrutura.Mapeamentos;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Contexto;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Patio> Filiais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MotoMapping());
        modelBuilder.ApplyConfiguration(new FilialMapping());
    }
}