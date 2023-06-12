namespace DependencyInjection.Fixtures; 

public class AnotherService : IAnotherService {
    private AnotherService() {}

    public static AnotherService Instance = new();
}