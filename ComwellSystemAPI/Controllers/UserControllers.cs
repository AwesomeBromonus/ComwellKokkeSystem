using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Repositories;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController()
        {
            _userRepo = new UserRepositoryMongodb();
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Brugernavn og adgangskode skal udfyldes");

            var existing = await _userRepo.GetByUsernameAsync(model.Username);
            if (existing != null)
                return Conflict("Brugernavn findes allerede");

            // Automatisk dato hvis ikke sat
            if (model.StartDato == default)
                model.StartDato = DateTime.UtcNow;

            await _userRepo.AddAsync(model);
            return Ok("Bruger oprettet");
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepo.GetByUsernameAsync(model.Username);
            if (user == null || user.Password != model.Password)
                return Unauthorized("Login fejlede");

            return Ok(new LoginResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            });
        }

        // GET: api/users/elevs
        [HttpGet("elevs")]
        public async Task<IActionResult> GetAllElevs()
        {
            var allUsers = await _userRepo.GetAllAsync();
            var elevs = allUsers.Where(u => u.Role == "elev").ToList();
            return Ok(elevs);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bruger = await _userRepo.GetByIdAsync(id);
            if (bruger == null)
                return NotFound();

            await _userRepo.DeleteAsync(id);
            return Ok("Bruger slettet");
        }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userRepo.GetAllAsync();
            return Ok(allUsers);
        }

    }
}
