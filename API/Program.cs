using Aplicacao.Servicos;
using Dominio.Interfaces;
using Dominio.Persistencia;
using DotNetEnv;
using Infraestrutura.Contexto;
using Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;


// TODO: Remover comentários inúteis de todas as classes
// TODO: Separar os pacotes por camadas de biblioteca de classes
// TODO: Adicionar camada SERVICE com interfaces

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    // TODO: Trocar para V2
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
    var connectionString = Environment.GetEnvironmentVariable("Connection__String");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseOracle(connectionString));
}
catch (ArgumentNullException)
{
    throw new Exception("Falha ao buscar a varíavel de ambiente");
}

// Register repositories with their interfaces
builder.Services.AddScoped<IRepositorio<Moto>, MotoRepositorio>();
builder.Services.AddScoped<IRepositorio<Filial>, FilialRepositorio>();

// Register services
builder.Services.AddScoped<MotoServico>();
builder.Services.AddScoped<FilialServico>();


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
