using DependencyInjection.Interfaces;

namespace DependencyInjection.Descriptors;

public class FactoryBasedServiceDescriptor : ServiceDescriptor {
    public Func<IScope, object> Factory { get; init; }
}