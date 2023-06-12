namespace DependencyInjection.Interfaces; 

public interface IContainer {
    IScope CreateScope();
}