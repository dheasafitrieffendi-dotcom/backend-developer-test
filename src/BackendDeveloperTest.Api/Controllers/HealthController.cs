using Microsoft.AspNetCore.Mvc;

namespace BackendDeveloperTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "OK",
            message = "Backend Developer Test API is running"
        });
    }
}