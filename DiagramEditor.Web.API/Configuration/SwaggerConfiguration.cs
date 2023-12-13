using System.Reflection;
using System.Runtime.CompilerServices;
using CSharpFunctionalExtensions;
using DiagramEditor.Application.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Vernou.Swashbuckle.HttpResultsAdapter;

namespace DiagramEditor.Web.API.Configuration;

internal static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(ConfigureSwaggerGen);

        return services;
    }

    private static void ConfigureSwaggerGen(SwaggerGenOptions options)
    {
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2595
        options.OperationFilter<HttpResultsOperationFilter>();

        options.SupportNonNullableReferenceTypes();

        options.DocumentFilter<MaybeDocumentFilter>();
        options.SchemaFilter<RequiredSchemaFilter>();
        options.OperationFilter<RequestBodyMediaOperationFilter>();

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
                    []
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

        return method.Name.Decapitalize();
    }
}

file sealed class RequiredSchemaFilter : ISchemaFilter
{
    private static readonly NullabilityInfoContext _nullabilityContext = new();

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var typeProperties = context.Type.GetProperties();

        schema.Required = new HashSet<string>();

        foreach (var schemeProperty in schema.Properties)
        {
            var property =
                typeProperties.SingleOrDefault(
                    x => x.Name.ToLower() == schemeProperty.Key.ToLower()
                )
                ?? throw new MissingFieldException(
                    $"Could not find property {schemeProperty.Key} in {context.Type}, or several names conflict"
                );

            var propertyType = property.PropertyType;

            var isMaybeProperty =
                propertyType.IsGenericType
                && propertyType.GetGenericTypeDefinition() == typeof(Maybe<>);

            var isNullable = IsMarkedAsNullable(property);

            if (!isMaybeProperty && !isNullable)
            {
                schema.Required.Add(schemeProperty.Key);
            }
        }
    }

    private static bool IsMarkedAsNullable(PropertyInfo property)
    {
        return _nullabilityContext.Create(property).WriteState is NullabilityState.Nullable;
    }
}

file sealed class MaybeDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        foreach (var schema in document.Components.Schemas)
        {
            FlattenMaybeProperties(schema.Value, context);
        }

        RemoveMaybeSchemas(document);
    }

    private void FlattenMaybeProperties(OpenApiSchema schema, DocumentFilterContext context)
    {
        foreach (var (key, value) in schema.Properties)
        {
            if (value.Reference is not { Id: var referenceId })
            {
                continue;
            }

            if (!IsReferenceToMaybe(referenceId, context.SchemaRepository))
            {
                continue;
            }

            var maybeSchema = context.SchemaRepository.Schemas[referenceId];

            if (!maybeSchema.Properties.TryGetValue("value", out var valueSchema))
            {
                throw new InvalidOperationException("Maybe schema must have a value property.");
            }

            schema.Properties[key] = valueSchema;
        }
    }

    private static bool IsReferenceToMaybe(string referenceId, SchemaRepository schemaRepository)
    {
        return IsMaybeSchema(schemaRepository.Schemas.First(x => x.Key == referenceId));
    }

    private static bool IsMaybeSchema(KeyValuePair<string, OpenApiSchema> referencedSchema)
    {
        return referencedSchema is { Key: var key, Value.Properties: var properties }
            && key.EndsWith("Maybe")
            && properties.Keys.ToArray() is ["value", "hasValue", "hasNoValue"];
    }

    private void RemoveMaybeSchemas(OpenApiDocument swaggerDoc)
    {
        swaggerDoc
            .Components
            .Schemas
            .Where(IsMaybeSchema)
            .ForEach(swaggerDoc.Components.Schemas.Remove);
    }
}

file sealed class RequestBodyMediaOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.RequestBody?.Content.Remove("application/*+json");
    }
}
