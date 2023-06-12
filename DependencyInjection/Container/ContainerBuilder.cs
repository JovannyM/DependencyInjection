using DependencyInjection.Descriptors;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Container; 

public class ContainerBuilder : IContainerBuilder {
    private readonly List<ServiceDescriptor> _descriptors = new();

    public void Register(ServiceDescriptor descriptor) {
        _descriptors.Add(descriptor);
    }

    public IContainer Build() {
        return new Container(_descriptors);
    }
}