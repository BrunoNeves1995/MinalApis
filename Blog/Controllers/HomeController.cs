using Blog.Atributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {   

        [HttpGet("")]
        // [ApiKeyAtribute]
        public IActionResult Get()
        {   
            return Ok( new {
                Status = "Online"
            });
        }
    }
}