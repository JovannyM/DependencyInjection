namespace DependencyInjection.Interfaces; 

public interface IScope {
    object Resolve(Type service);
}