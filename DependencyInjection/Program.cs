using DependencyInjection.Container;
using DependencyInjection.Fixtures;
using DependencyInjection.Interfaces;

IContainerBuilder builder = new ContainerBuilder();
var container = builder
    .RegisterTransient<IService, Service>()
    .RegisterScoped<Controller, Controller>()
    .Build();

var scope = container.CreateScope();
var controller1 = scope.Resolve(typeof(Controller));
var controller2 = scope.Resolve(typeof(Controller));
if (controller1 != controller2) {
    throw new InvalidOperationException();
}