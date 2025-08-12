using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;

namespace SMJRegisterAPI.Swagger;

public class BearerSecuritySchemeTransformer : IDocumentFilter
{
    private readonly IAuthenticationSchemeProvider _schemeProvider;

    public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider schemeProvider)
    {
        _schemeProvider = schemeProvider;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var scheme = _schemeProvider.GetSchemeAsync("Bearer").Result;
        if (scheme != null)
        {
            swaggerDoc.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            foreach (var path in swaggerDoc.Paths.Values)
            {
                foreach (var operation in path.Operations.Values)
                {
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                }
            }
        }
    }
}