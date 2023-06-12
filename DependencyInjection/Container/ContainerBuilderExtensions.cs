using DependencyInjection.Descriptors;
using DependencyInjection.Interfaces;
using DependencyInjection.Types;

namespace DependencyInjection.Container; 

public static class ContainerBuilderExtensions {
    private static IContainerBuilder RegisterType(
        this IContainerBuilder builder,
        Type service,
        Type implementation,
        LifeTime lifeTime
    ) {
        builder.Register(new TypeBasedServiceDescriptor() {
            ImplementationType = implementation,
            ServiceType = service,
            LifeTime = lifeTime
        });
        return builder;
    }

    private static IContainerBuilder RegisterFactory(this IContainerBuilder builder, Type service,
        Func<IScope, object> factory, LifeTime lifeTime) {
        builder.Register(new FactoryBasedServiceDescriptor() {
            Factory = factory,
            ServiceType = service,
            LifeTime = lifeTime
        });
        return builder;
    }

    private static IContainerBuilder RegisterInstance(this IContainerBuilder builder, Type service, object instance) {
        builder.Register(new InstanceBasedServiceDescriptor(service, instance));
        return builder;
    }

    // [Interface\Class] (With signature params) registration
    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type @serviceInterface,
        Type serviceImplementation) =>
        builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Singleton);

    public static IContainerBuilder RegisterTransient(this IContainerBuilder builder, Type serviceInterface,
        Type serviceImplementation) =>
        builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Transient);

    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type serviceInterface,
        Type serviceImplementation) =>
        builder.RegisterType(serviceInterface, serviceImplementation, LifeTime.Scoped);

    // [Interface\Class] (With generic params) registration
    public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService =>
        builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Singleton);

    public static IContainerBuilder RegisterTransient<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService =>
        builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Transient);

    public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder)
        where TImplementation : TService =>
        builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Scoped);

    // [Interface/factory] registration
    public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type serviceInterface,
        Func<IScope, object> factory) =>
        builder.RegisterFactory(serviceInterface, factory, LifeTime.Singleton);

    //TODO: Possible to refactor like RegisterSingleton
    public static IContainerBuilder RegisterTransient<TService>(this IContainerBuilder builder,
        Func<IScope, TService> factory) =>
        builder.RegisterFactory(typeof(TService), (s) => factory(s), LifeTime.Transient);

    //TODO: Possible to refactor like RegisterSingleton
    public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type serviceInterface,
        Func<IScope, object> factory) =>
        builder.RegisterFactory(serviceInterface, factory, LifeTime.Scoped);

    // [Interface/instance] registration
    public static IContainerBuilder RegisterSingleton<TService>(this IContainerBuilder builder, object instance) =>
        builder.RegisterInstance(typeof(TService), instance);
}