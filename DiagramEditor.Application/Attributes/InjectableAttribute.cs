using Microsoft.Extensions.DependencyInjection;

namespace DiagramEditor.Application.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class InjectableAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}
