namespace DependencyInjection.Interfaces; 

public interface IScope: IDisposable, IAsyncDisposable {
    object Resolve(Type service);
}