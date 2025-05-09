using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Midlertidig brugerliste
        private static List<UserModel> users = new List<UserModel>();

        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            if (users.Any(u => u.Username == model.Username))
                return BadRequest("Brugernavn er allerede i brug");

            var user = new UserModel
            {
                Id = users.Count + 1,
                Username = model.Username,
                Password = model.Password, // OBS: Klartekst – kun til test
                Role = model.Role
            };

            users.Add(user);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user == null)
                return Unauthorized("Forkert brugernavn eller adgangskode");

            // 💥 Returner brugerdata som JSON
            return Ok(new
            {
                user.Id,
                user.Username,
                user.Role
            });
        }
    }
}
