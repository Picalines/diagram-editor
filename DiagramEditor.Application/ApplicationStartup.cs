using DiagramEditor.Application.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application;

public static class ApplicationStartup
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddInjectableFromCallingAssembly();

        return services;
    }
}
