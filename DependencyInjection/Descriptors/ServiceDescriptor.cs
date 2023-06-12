using DependencyInjection.Types;

namespace DependencyInjection.Descriptors;

public abstract class ServiceDescriptor {
    public Type ServiceType { get; init; }
    public LifeTime LifeTime { get; init; }
}