using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DiagramEditor.Web.API.Filters;

namespace DiagramEditor.Web.API.Configuration;

internal static class ControllersConfiguration
{
    public static IServiceCollection AddConfiguredControllers(this IServiceCollection services)
    {
        var mvcBuilder = services.AddControllers(options =>
        {
            options.Filters.Add<ModelStateActionFilter>();
        });

        mvcBuilder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new MaybeJsonConverter());

            options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { MaybeJsonConverter.Modifier }
            };
        });

        mvcBuilder.AddHybridModelBinder();

        return services;
    }
}
