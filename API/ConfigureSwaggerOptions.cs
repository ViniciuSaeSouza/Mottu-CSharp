using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "API de filiais e motos Mottu",
                    Version = description.ApiVersion.ToString(),
                    Description = "API para gerenciar filiais e motos da Mottu nos pátios",
                    Contact = new OpenApiContact
                    {
                        Name = "Prisma.Code",
                        Email = "prismacode3@gmail.com"
                    }
                });
            }
        }
    }
}

