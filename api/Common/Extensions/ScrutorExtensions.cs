using DiagramEditor.Attributes;
using Scrutor;

namespace DiagramEditor.Common.Extensions;

public static class ScrutorExtensions
{
    public static IImplementationTypeSelector InjectableAttribute(
        this IImplementationTypeSelector selector,
        ServiceLifetime lifeTime
    )
    {
        return selector
            .AddClasses(c => c.WithAttribute<InjectableAttribute>(i => i.Lifetime == lifeTime))
            .AsImplementedInterfaces()
            .AsSelf()
            .WithLifetime(lifeTime);
    }

    public static IImplementationTypeSelector InjectableAttributes(
        this IImplementationTypeSelector selector
    )
    {
        foreach (var item in Enum.GetValues<ServiceLifetime>())
        {
            selector = selector.InjectableAttribute(item);
        }

        return selector;
    }
}
