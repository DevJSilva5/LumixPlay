using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SwaggerController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            mensagem = "Swagger funcionando",
            status = true
        });
    }

    [HttpGet("filme")]
    public IActionResult Filme()
    {
        return Ok(new
        {
            titulo = "Batman",
            ano = 2022
        });
    }
}