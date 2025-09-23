using Dominio.Persistencia;
using Dominio.Persistencia.Mottu;
using Infraestrutura.Mapeamentos;
using Infraestrutura.Mapeamentos.Mottu;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Contexto;

public class AppDbContext : DbContext
{
    
    public DbSet<Moto> Motos { get; set; }
    public DbSet<Patio> Patios { get; set; }
    public DbSet<MotoMottu> MotosMottu { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MotoMapping());
        modelBuilder.ApplyConfiguration(new PatioMapping());
        modelBuilder.ApplyConfiguration(new MotoMottuMapping());
    }
}