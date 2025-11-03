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
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using API.Servicos;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Dominio.Interfaces.Mottu;
using Infraestrutura.Repositorios.Mottu;

// TODO: adicionar logica de vinculo usuario patio
// TODO: adicionar logica de moto com patio
// TODO: adicionar logica de moto com carrapato
// TODO: adicionar logica de carrapato com patio

Env.Load();

var builder = WebApplication.CreateBuilder(args);

string? connectionString = null;

// Add services to the container.
builder.Services.AddControllers();

// JWT Configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSeguraParaJWT123456";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MottuAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "MottuAPI";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(5), // Allow 5 minutes clock skew
            RequireExpirationTime = true,
            RequireSignedTokens = true
        };
    });

builder.Services.AddAuthorization();

// API Versioning
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(2, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("version"),
        new HeaderApiVersionReader("X-Version"),
        new UrlSegmentApiVersionReader()
    );
}).AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddHealthChecks();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API de filiais e motos Mottu",
        Version = "v1",
        Description = "API para gerenciar filiais e motos da Mottu nos pátios - Versão 1",
        Contact = new OpenApiContact
        {
            Name = "Prisma.Code",
            Email = "prismacode3@gmail.com"
        },
    });

    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "API de filiais e motos Mottu",
        Version = "v2",
        Description = "API para gerenciar filiais e motos da Mottu nos pátios - Versão 2 com ML.NET e JWT",
        Contact = new OpenApiContact
        {
            Name = "Prisma.Code",
            Email = "prismacode3@gmail.com"
        },
    });

    // JWT Configuration for Swagger
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "bearer",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    swagger.IncludeXmlComments(xmlPath);

    // Inclui controllers nas docs corretas por versão
    swagger.DocInclusionPredicate((docName, apiDesc) =>
    {
        // Use the ApiExplorer-assigned GroupName (e.g. "v1") to decide which actions belong to each Swagger doc.
        // apiDesc.GroupName is populated by the VersionedApiExplorer when AddVersionedApiExplorer is registered.
        if (!string.IsNullOrEmpty(apiDesc.GroupName))
            return string.Equals(apiDesc.GroupName, docName, StringComparison.OrdinalIgnoreCase);

        // If no group name is present, include the endpoint only in the default doc (optional).
        return false;
    });
});

try
{
    connectionString = Environment.GetEnvironmentVariable("Connection__String") ??
                           builder.Configuration.GetConnectionString("Oracle");
    
    if (string.IsNullOrEmpty(connectionString))
        throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null or empty");
        
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
builder.Services.AddScoped<IRepositorioUsuario, UsuarioRepositorio>();
builder.Services.AddScoped<IRepositorioCarrapato, CarrapatoRepositorio>();
builder.Services.AddScoped<IMottuRepositorio, MotoMottuRepositorio>();

// Injeção de serviços
builder.Services.AddScoped<MotoServico>();
builder.Services.AddScoped<PatioServico>();
builder.Services.AddScoped<UsuarioServico>();
builder.Services.AddScoped<CarrapatoServico>();

// JWT e ML.NET Services
builder.Services.AddScoped<JwtService>();

// HealthCheck
builder.Services.AddHealthChecks()
    .AddOracle(
        connectionString: connectionString!,
        name: "oracle-db",
        tags: new[] { "ready", "oracle", "database" })
    .AddCheck<CarrapatoHealthCheck>("carrapato_repositorio", tags: new[] { "ready" })
    .AddCheck("api-health", () => HealthCheckResult.Healthy("API está funcionando corretamente"), tags: new[] { "ready" });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "API de filiais e motos Mottu v2");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de filiais e motos Mottu v1");
    c.DefaultModelsExpandDepth(1);
    c.DisplayRequestDuration();
});

// HTTPS Redirection - but allow JWT authentication to work first
app.UseRouting();

// CORS - Add this to support HTTPS requests
app.UseCors(policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader();
});

// Authentication MUST come before HTTPS redirection for JWT to work properly
app.UseAuthentication();
app.UseAuthorization();

// HTTPS redirection after authentication
// Enable HTTPS redirection in production to prevent man-in-the-middle attacks.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Health Checks endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Liveness: retorna 200 se o app estiver rodando
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


app.MapControllers();

app.Run();

public partial class Program { }
