using Microsoft.AspNetCore.Mvc;

namespace SilentLux.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        public IActionResult Get()
        {
            return Ok("Hello world from Home");
        }
    }
}