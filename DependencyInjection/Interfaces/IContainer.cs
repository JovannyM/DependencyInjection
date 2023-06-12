namespace DependencyInjection.Interfaces; 

public interface IContainer : IDisposable, IAsyncDisposable {
    IScope CreateScope();
}