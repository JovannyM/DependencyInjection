namespace DependencyInjection.Fixtures; 

public class Controller : IController {
    private readonly IService _service;
    public Controller(IService service) {
        _service = service;
    }
}