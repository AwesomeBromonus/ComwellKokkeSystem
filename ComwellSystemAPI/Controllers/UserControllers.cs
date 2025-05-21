using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Brugernavn og adgangskode skal udfyldes.");

            var existing = await _userRepo.GetByUsernameAsync(model.Username);
            if (existing != null)
                return Conflict("Brugernavn findes allerede.");

            model.Role = model.Role?.Trim().ToLower();

            if (model.StartDato == default)
                model.StartDato = DateTime.UtcNow;

            await _userRepo.AddAsync(model);
            return Ok(new { message = "Bruger oprettet." });
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Udfyld brugernavn og adgangskode.");

            var user = await _userRepo.GetByUsernameAsync(model.Username);
            if (user == null || user.Password != model.Password)
                return Unauthorized("Forkert brugernavn eller adgangskode.");

            var response = new LoginResponse
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                HotelId = user.HotelId,
                ElevplanId = user.ElevplanId
            };

            return Ok(response);
        }

        // GET: api/users/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userRepo.GetAllAsync();
            return Ok(allUsers);
        }

        [HttpGet("admins-og-kokke")]
        public async Task<IActionResult> GetAdminsOgKokke()
        {
            var brugere = await _userRepo.GetAdminsOgKokkeAsync();
            return Ok(brugere);
        }


        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bruger = await _userRepo.GetByIdAsync(id);
            if (bruger == null)
                return NotFound("Bruger ikke fundet.");

            await _userRepo.DeleteAsync(id);
            return Ok("Bruger slettet.");
        }

        // PUT: api/users/{id}/assign-elevplan
        [HttpPut("{id}/assign-elevplan")]
        public async Task<IActionResult> AssignElevplan(int id, [FromBody] AssignElevplanRequest request)
        {
            var bruger = await _userRepo.GetByIdAsync(id);
            if (bruger == null)
                return NotFound("Bruger ikke fundet.");

            bruger.ElevplanId = request.ElevplanId;
            await _userRepo.UpdateUserAsync(bruger);

            return Ok("Elevplan tildelt.");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserModel bruger)
        {
            var eksisterende = await _userRepo.GetByIdAsync(id);
            if (eksisterende == null)
                return NotFound("Bruger ikke fundet.");

            bruger.Id = id; // sikrer korrekt ID
            await _userRepo.UpdateUserAsync(bruger);

            return Ok("Bruger opdateret.");
        }
        // GET: api/users/{username}
        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null)
                return NotFound("Bruger ikke fundet.");

            return Ok(user);
        }
        [HttpGet("elever/{year}")]
        public async Task<ActionResult<List<UserModel>>> GetEleverByYear(int year)
        {
            var allUsers = await _userRepo.GetAllAsync();
            var elever = allUsers
                .Where(u => u.Role == "elev" && u.StartDato.Year == year)
                .ToList();
            return Ok(elever);
        }
        
        [HttpGet("byid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                return NotFound("Bruger ikke fundet.");
            return Ok(user);
        }

    }
    

    public class AssignElevplanRequest
    {
        public int ElevplanId { get; set; }
    }
}
