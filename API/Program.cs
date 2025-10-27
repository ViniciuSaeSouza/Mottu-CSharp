using Aplicacao.Servicos;
using Dominio.Interfaces;
using Dominio.Persistencia;
using DotNetEnv;
using Infraestrutura.Contexto;
using Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using API.Saude;
using Aplicacao.Servicos.Mottu;
using Dominio.Interfaces.Mottu;
using HealthChecks.UI.Client;
using Infraestrutura.Repositorios.Mottu;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

// TODO: adicionar logica de vinculo usuario patio
// TODO: adicionar logica de moto com patio
// TODO: adicionar logica de moto com carrapato
// TODO: adicionar logica de carrapato com patio

Env.Load();

var builder = WebApplication.CreateBuilder(args);

string? connectionString = null;

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "API de filiais e motos Mottu",
        Version = "v2",
        Description = "API para gerenciar filiais e motos da Mottu nos pátios",
        Contact = new OpenApiContact
        {
            Name = "Prisma.Code",
            Email = "prismacode3@gmail.com"
        },
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    swagger.IncludeXmlComments(xmlPath);
});


try
{
    connectionString = Environment.GetEnvironmentVariable("ConnectionString__Oracle") ??
                           builder.Configuration.GetConnectionString("Oracle");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(connectionString));
}
catch (ArgumentNullException)
{
    throw new Exception("Falha ao buscar a variável de ambiente");
}

// Injeção de repositórios
builder.Services.AddScoped<IMotoRepositorio, MotoRepositorio>();
builder.Services.AddScoped<IRepositorio<Patio>, PatioRepositorio>();
builder.Services.AddScoped<IMottuRepositorio, MotoMottuRepositorio>();
builder.Services.AddScoped<IRepositorioUsuario, UsuarioRepositorio>();
builder.Services.AddScoped<IRepositorioCarrapato, CarrapatoRepositorio>();

// Injeção de serviços
builder.Services.AddScoped<MotoServico>();
builder.Services.AddScoped<PatioServico>();
builder.Services.AddScoped<MotoMottuServico>();
builder.Services.AddScoped<UsuarioServico>();
builder.Services.AddScoped<CarrapatoServico>();

// HealthCheck
builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: connectionString,
        name: "Oracle",
        tags: new[] { "ready", "oracle-database" })
    .AddCheck<CarrapatoHealthCheck>("carrapato_repositorio", tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "API de filiais e motos Mottu v2");
    });
}

app.UseHttpsRedirection();


// Liveness: retorna 200 so se o app estiver rodando
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // sem check de dependencia, so liveness
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Readiness: verifica dependencias com tag "ready"
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthorization();

app.MapControllers();

app.Run();