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
        private readonly IWebHostEnvironment _env;

        public UsersController(IUserRepository userRepo, IWebHostEnvironment env)
        {
            _userRepo = userRepo;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserModel user)
        {
            await _userRepo.AddAsync(user);
            return Ok();
        }


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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest("Udfyld brugernavn og adgangskode.");

            var user = await _userRepo.GetByUsernameAsync(model.Username);
            if (user == null || user.Password != model.Password)
                return Unauthorized("Forkert brugernavn eller adgangskode.");

            var response = new UserModel
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role,
                HotelId = user.HotelId,
                ElevplanId = user.ElevplanId,
                Navn = user.Navn,
                Email = user.Email
            };

            return Ok(response);
        }

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

        [HttpGet("byid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                return NotFound("Bruger ikke fundet.");
            return Ok(user);
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserModel bruger)
        {
            var eksisterende = await _userRepo.GetByIdAsync(id);
            if (eksisterende == null)
                return NotFound("Bruger ikke fundet.");

            bruger.Id = id;
            await _userRepo.UpdateUserAsync(bruger);

            return Ok("Bruger opdateret.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bruger = await _userRepo.GetByIdAsync(id);
            if (bruger == null)
                return NotFound("Bruger ikke fundet.");

            await _userRepo.DeleteAsync(id);
            return Ok("Bruger slettet.");
        }

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

        [HttpPut("{id}/skiftkode")]
        public async Task<IActionResult> SkiftAdgangskode(int id, [FromBody] string nyAdgangskode)
        {
            var bruger = await _userRepo.GetByIdAsync(id);
            if (bruger == null)
                return NotFound("Bruger ikke fundet.");

            if (string.IsNullOrWhiteSpace(nyAdgangskode))
                return BadRequest("Adgangskode kan ikke være tom.");

            bruger.Password = nyAdgangskode;
            await _userRepo.UpdateUserAsync(bruger);

            return Ok("Adgangskode opdateret.");
        }

        [HttpPost("{id}/upload-billede")]
        public async Task<IActionResult> UploadProfilbillede(int id)
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file == null || file.Length == 0)
                    return BadRequest("Ingen fil modtaget.");

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, $"{id}.jpg");

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok("Billede uploadet.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fejl ved upload: {ex.Message}");
            }
        }

        [HttpGet("{id}/eksisterer-billede")]
        public IActionResult HarProfilbillede(int id)
        {
            var filePath = Path.Combine(_env.WebRootPath, "uploads", $"{id}.jpg");
            bool exists = System.IO.File.Exists(filePath);
            return Ok(new { exists });
        }
    }

    public class AssignElevplanRequest
    {
        public int ElevplanId { get; set; }
    }
}
