using System.Reflection;
using DiagramEditor.Application.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace DiagramEditor.Application.Extensions;

public static class ScrutorExtensions
{
    public static IImplementationTypeSelector InjectableAttribute(this IImplementationTypeSelector selector, ServiceLifetime lifeTime)
    {
        return selector
            .AddClasses(c => c.WithAttribute<InjectableAttribute>(i => i.Lifetime == lifeTime))
            .AsImplementedInterfaces()
            .AsSelf()
            .WithLifetime(lifeTime);
    }

    public static IImplementationTypeSelector InjectableAttributes(this IImplementationTypeSelector selector)
    {
        foreach (var item in Enum.GetValues<ServiceLifetime>())
        {
            selector = selector.InjectableAttribute(item);
        }

        return selector;
    }

    public static IServiceCollection AddInjectableFromCallingAssembly(this IServiceCollection services)
    {
        var assembly = Assembly.GetCallingAssembly();
        return services.Scan(selector => selector.FromAssemblies(assembly).InjectableAttributes());
    }
}
