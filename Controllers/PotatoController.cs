using Microsoft.AspNetCore.Mvc;
using PotatoCounter.Interfaces;

namespace PotatoCounter.Controllers;

[ApiController]
[Route("[controller]")]
public class PotatoController : ControllerBase
{
   int potatoCounter = 0;
    private readonly ILogger<PotatoController> _logger;
    private readonly IPotatoPostSender _mqHandler;

    public PotatoController(ILogger<PotatoController> logger, IPotatoPostSender mqHandler)
    {
        _logger = logger;
        _mqHandler = mqHandler;
    }

    [HttpGet]
    public int Get()
    {
        _mqHandler.SendPotatoPost(new Models.Potato{Count = 1, Origin = "Test", Name = "User" });
        potatoCounter++;
        return potatoCounter;
    }
    
}
