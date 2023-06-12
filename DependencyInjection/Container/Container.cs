using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Reflection;
using DependencyInjection.Descriptors;
using DependencyInjection.Interfaces;
using DependencyInjection.Types;

namespace DependencyInjection.Container;

public class Container : IContainer {
    private readonly ImmutableDictionary<Type, ServiceDescriptor> _descriptors;
    private readonly ConcurrentDictionary<Type, Func<IScope, object>> _buildActivators = new();
    private readonly Scope _rootScope;

    public Container(IEnumerable<ServiceDescriptor> descriptors) {
        _descriptors = descriptors.ToImmutableDictionary(x => x.ServiceType);
        _rootScope = new(this);
    }

    private class Scope : IScope {
        private readonly Container _container;
        private readonly ConcurrentDictionary<Type, object> _scopedInstances = new();

        public Scope(Container container) {
            _container = container;
        }

        public object Resolve(Type service) {
            var descriptor = _container.FindDescriptor(service);
            if (descriptor.LifeTime == LifeTime.Transient)
                return _container.CreateInstance(service, this);
            if (descriptor.LifeTime == LifeTime.Scoped || _container._rootScope == this)
                return _scopedInstances.GetOrAdd(service, s => _container.CreateInstance(s, this));
            else return _container._rootScope.Resolve(service);
        }
    }

    public IScope CreateScope() {
        return new Scope(this);
    }

    private ServiceDescriptor? FindDescriptor(Type service) {
        _descriptors.TryGetValue(service, out var result);
        return result;
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