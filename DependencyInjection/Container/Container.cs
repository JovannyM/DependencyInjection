using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using DependencyInjection.Descriptors;
using DependencyInjection.Interfaces;

namespace DependencyInjection.Container; 

public class Container : IContainer {

    private readonly ImmutableDictionary<Type, ServiceDescriptor> _descriptors;
    private readonly ConcurrentDictionary<Type, Func<IScope, object>> _buildActivators = new();

    public Container(IEnumerable<ServiceDescriptor> descriptors) {
        _descriptors = descriptors.ToImmutableDictionary(x => x.ServiceType);
    }

    private class Scope : IScope {

        private readonly Container _container;
        
        public Scope(Container container) {
            _container = container;
        }

        public object Resolve(Type service) => _container.CreateInstance(service, this);
    }
    
    public IScope CreateScope() {
        return new Scope(this);
    }

    private Func<IScope, object> BuildActivation(Type service) {
        if (!_descriptors.TryGetValue(service, out var descriptor))
            throw new InvalidOperationException($"Service {service} is not registered");

        if (descriptor is InstanceBasedServiceDescriptor ib)
            return _ => ib.Instance;
        
        if (descriptor is FactoryBasedServiceDescriptor fb) 
            return fb.Factory;
        
        var tb = (TypeBasedServiceDescriptor)descriptor;

        var constructor = tb.ImplementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Single();
        var args = constructor.GetParameters();

        return (scope) => {
            var argsForCtor = new object[args.Length];
            for (int i = 0; i < args.Length; i++) {
                argsForCtor[i] = CreateInstance(args[i].ParameterType, scope);
            }

            return constructor.Invoke(argsForCtor);
        };
    }

    private object CreateInstance(Type service, IScope scope) {
        return _buildActivators.GetOrAdd(service, BuildActivation)(scope);
    }
}