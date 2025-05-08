using Microsoft.AspNetCore.Mvc;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElevplanController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTest()
        {
            return Ok("ElevplanController virker!");
        }
    }
}
