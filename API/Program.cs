using Aplicacao.Servicos;
using Dominio.Interfaces;
using Dominio.Persistencia;
using DotNetEnv;
using Infraestrutura.Contexto;
using Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Aplicacao.Servicos.Mottu;
using Dominio.Interfaces.Mottu;
using Dominio.Persistencia.Mottu;
using Infraestrutura.Repositorios.Mottu;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de filiais e motos Mottu",
        Version = "v1",
        Description = "API para gerenciar filiais e motos da Mottu nos p�tios",
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
    var connectionString = Environment.GetEnvironmentVariable("Connection__String") ??
                           builder.Configuration.GetConnectionString("Oracle");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(connectionString));
}
catch (ArgumentNullException)
{
    throw new Exception("Falha ao buscar a variável de ambiente");
}

// Injeção de repositórios
builder.Services.AddScoped<IRepositorio<Moto>, MotoRepositorio>();
builder.Services.AddScoped<IRepositorio<Patio>, PatioRepositorio>();
builder.Services.AddScoped<IMottuRepositorio, MotoMottuRepositorio>();
builder.Services.AddScoped<IRepositorio<Usuario>, UsuarioRepositorio>();


// Injeção de serviços
builder.Services.AddScoped<MotoServico>();
builder.Services.AddScoped<PatioServico>();
builder.Services.AddScoped<MotoMottuServico>();
builder.Services.AddScoped<UsuarioServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();