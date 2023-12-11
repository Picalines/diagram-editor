using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace DiagramEditor.Web.API.Configuration;

internal static class JsonConfiguration
{
    public static IMvcBuilder AddJsonOptions(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new MaybeJsonConverter());

            options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers = { MaybeJsonConverter.Modifier }
            };
        });

        return mvcBuilder;
    }
}
