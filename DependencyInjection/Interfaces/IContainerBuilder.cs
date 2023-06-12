using DependencyInjection.Descriptors;

namespace DependencyInjection.Interfaces; 

public interface IContainerBuilder {
    void Register(ServiceDescriptor descriptor);

    IContainer Build();
}