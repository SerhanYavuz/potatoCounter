using Microsoft.AspNetCore.Mvc;

namespace PotatoCounter.Controllers;

[ApiController]
[Route("[controller]")]
public class PotatoController : ControllerBase
{
   int potatoCounter = 0;
    private readonly ILogger<PotatoController> _logger;

    public PotatoController(ILogger<PotatoController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public int Get()
    {
        potatoCounter++;
        return potatoCounter;
    }
    
}
