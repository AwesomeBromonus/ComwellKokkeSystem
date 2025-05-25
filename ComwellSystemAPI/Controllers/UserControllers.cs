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

        [HttpPut("{id}/skiftkode")]
        public async Task<IActionResult> SkiftAdgangskode(int id, [FromBody] string nyAdgangskode)
        {
            var bruger = await _userRepo.GetByIdAsync(id);
            if (bruger == null)
                return NotFound("Bruger ikke fundet.");

            if (string.IsNullOrWhiteSpace(nyAdgangskode))
                return BadRequest("Adgangskode kan ikke være tom.");

            // OBS: Hash hvis ønsket
            bruger.Password = nyAdgangskode;
            await _userRepo.UpdateUserAsync(bruger);

            return Ok("Adgangskode opdateret.");
        }

        [HttpPost("{id}/upload-billede")]
        public async Task<IActionResult> UploadProfilbillede(int id)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
                return BadRequest("Ingen fil modtaget.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, $"{id}.jpg");

            try
            {
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
}
