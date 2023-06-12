using DependencyInjection.Types;

namespace DependencyInjection.Descriptors;

public class InstanceBasedServiceDescriptor : ServiceDescriptor {
    public object Instance { get; init; }

    public InstanceBasedServiceDescriptor(Type serviceType, object instance) {
        ServiceType = serviceType;
        LifeTime = LifeTime.Singleton;
        Instance = instance;
    }
}