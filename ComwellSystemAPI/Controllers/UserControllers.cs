using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Repositories;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepositoryMongodb _userRepo;

        public UsersController()
        {
            _userRepo = new UserRepositoryMongodb();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var existing = await _userRepo.GetByUsernameAsync(model.Username);
            if (existing != null)
                return Conflict("Brugernavn findes allerede");

            var user = new UserModel
            {
                Username = model.Username,
                Password = model.Password,
                Role = model.Role
            };

            await _userRepo.AddAsync(user);
            return Ok("Bruger oprettet");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var isValid = await _userRepo.ValidateLogin(model.Username, model.Password);
            if (!isValid)
                return Unauthorized("Login fejlede");

            var user = await _userRepo.GetByUsernameAsync(model.Username);
            return Ok(new LoginResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}
