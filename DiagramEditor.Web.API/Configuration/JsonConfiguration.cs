using System.Text.Json.Serialization;

namespace DiagramEditor.Web.API.Configuration;

internal static class JsonConfiguration
{
    public static IMvcBuilder AddJsonOptions(this IMvcBuilder mvcBuilder)
    {
        mvcBuilder.AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return mvcBuilder;
    }
}
