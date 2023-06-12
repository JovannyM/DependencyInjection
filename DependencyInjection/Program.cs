using DependencyInjection.Container;
using DependencyInjection.Fixtures;
using DependencyInjection.Interfaces;

IContainerBuilder builder = new ContainerBuilder();
var container = builder
    .RegisterTransient<IService, Service>()
    .RegisterScoped<Controller, Controller>()
    .RegisterSingleton<IAnotherService>(AnotherService.Instance)
    .Build();

var scope = container.CreateScope();
var controller1 = scope.Resolve(typeof(Controller));
var controller2 = scope.Resolve(typeof(Controller));
var scope2 = container.CreateScope();
var controller3 = scope2.Resolve(typeof(Controller));
var i1 = scope.Resolve(typeof(IAnotherService));
var i2 = scope2.Resolve(typeof(IAnotherService));

if (controller1 != controller2) {
    throw new InvalidOperationException();
}
if (controller1 == controller3) {
    throw new InvalidOperationException();
}
if (i1 != i2) {
    throw new InvalidOperationException();
}

Console.WriteLine("All works fine!");