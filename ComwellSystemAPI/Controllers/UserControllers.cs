using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // Midlertidig liste til at simulere brugere (erstattes senere af database)
        private static List<UserModel> users = new List<UserModel>();

        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            // Tjek om brugernavn allerede findes
            if (users.Any(u => u.Username == model.Username))
                return BadRequest("Brugernavn er allerede i brug");

            // Opret ny bruger og tilføj til listen
            var user = new UserModel
            {
                Id = users.Count + 1,
                Username = model.Username,
                Password = model.Password, // OBS: gemmes i klartekst (kun til test)
                Role = model.Role
            };

            users.Add(user);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            // Tjek om bruger findes og kodeord matcher
            var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
                return Unauthorized("Forkert brugernavn eller adgangskode");

            // Returnér brugerens ID for at simulere login
            return Ok(user.Id);
        }
    }
}
