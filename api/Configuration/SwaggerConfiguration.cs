using System.Runtime.CompilerServices;
using DiagramEditor.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Vernou.Swashbuckle.HttpResultsAdapter;

namespace DiagramEditor.Configuration;

public static class SwaggerConfiguration
{
    public static void UseSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(ConfigureSwaggerGen);
    }

    private static void ConfigureSwaggerGen(SwaggerGenOptions options)
    {
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2595
        options.OperationFilter<HttpResultsOperationFilter>();

        options.SupportNonNullableReferenceTypes();

        options.SchemaFilter<SwaggerRequiredSchemaFilter>();
        options.OperationFilter<SwaggerRequestBodyMediaFilter>();

        options.CustomOperationIds(GetOperationId);

        options.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter a valid JSON web token here",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            }
        );

        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
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
                    [ ]
                }
            }
        );
    }

    private static string? GetOperationId(ApiDescription apiDescription)
    {
        if (apiDescription.TryGetMethodInfo(out var method) is false)
        {
            return null;
        }

        var operationId = method
            .Name
            .Capitalize()
            .AddPrefix(apiDescription.HttpMethod?.ToLower().Capitalize() ?? "")
            .Decapitalize();

        if (method.DeclaringType is { Name: var controllerTypeName })
        {
            controllerTypeName = controllerTypeName.RemoveSuffix("Controller").Capitalize();

            if (operationId.Contains(controllerTypeName) is false)
            {
                operationId = operationId.AddSuffix(controllerTypeName);
            }
        }

        return operationId;
    }
}

file sealed class SwaggerRequiredSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var typeProperties = context.Type.GetProperties();

        foreach (var schemeProperty in schema.Properties)
        {
            var typeProperty =
                typeProperties.SingleOrDefault(
                    x => x.Name.ToLower() == schemeProperty.Key.ToLower()
                )
                ?? throw new MissingFieldException(
                    $"Could not find property {schemeProperty.Key} in {context.Type}, or several names conflict"
                );

            if (typeProperty.IsDefined(typeof(RequiredMemberAttribute), false))
            {
                schema.Required.Add(schemeProperty.Key);
            }
        }
    }
}

file sealed class SwaggerRequestBodyMediaFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        Console.WriteLine(string.Join(", ", operation.RequestBody?.Content.Keys ?? new[] {""}));
        operation.RequestBody?.Content.Remove("application/*+json");
    }
}
