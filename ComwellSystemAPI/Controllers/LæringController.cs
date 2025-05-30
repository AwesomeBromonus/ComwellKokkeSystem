using Microsoft.AspNetCore.Mvc;
using Modeller;
using Interface;

namespace ComwellSystemAPI.Controllers
{
    // API-controller til håndtering af læringsmaterialer
    [ApiController]
    [Route("api/laering")]
    public class LæringController : ControllerBase
    {
        private readonly ILæring _repo;

        // Konstruktor hvor repository injiceres til datatilgang
        public LæringController(ILæring repo)
        {
            _repo = repo;
        }

        // GET: api/laering
        // Henter alle læringsmaterialer asynkront
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var laeringer = await _repo.GetAllAsync();
            return Ok(laeringer);
        }

        // GET: api/laering/{id}
        // Henter et enkelt læringsmateriale baseret på id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var læring = await _repo.GetByIdAsync(id);
            if (læring == null)
                return NotFound();

            return Ok(læring);
        }

        // POST: api/laering
        // Tilføjer et nyt læringsmateriale til databasen
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Læring læring)
        {
            if (læring == null)
                return BadRequest("Læring er null");

            await _repo.AddAsync(læring);
            return Ok(læring);
        }

        // POST: api/laering/upload
        // Håndterer upload af fil sammen med titel og beskrivelse
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string titel, [FromForm] string beskrivelse)
        {
            try
            {
                // Definerer hvilke filtyper, der er tilladt
                var tilladteTyper = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".mp4" };
                var extension = Path.GetExtension(file.FileName).ToLower();

                // Validerer at en fil er valgt
                if (file == null || file.Length == 0)
                    return BadRequest("Ingen fil valgt.");

                // Validerer at filtypen er tilladt
                if (!tilladteTyper.Contains(extension))
                    return BadRequest("Filtypen er ikke tilladt.");

                // Læser filen ind i en MemoryStream og konverterer til base64
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var filBytes = ms.ToArray();
                var base64 = Convert.ToBase64String(filBytes);

                // Opretter et nyt Læring-objekt med de modtagne data
                var læring = new Læring
                {
                    Titel = titel,
                    Beskrivelse = beskrivelse,
                    FilNavn = file.FileName,
                    FilSti = "",
                    Filtype = extension.Trim('.'),
                    FilIndholdBase64 = base64,
                    Oprettet = DateTime.UtcNow
                };

                // Gemmer læringsmaterialet i databasen
                await _repo.AddAsync(læring);
                return Ok(læring);
            }
            catch (Exception ex)
            {
                // Returnerer fejl hvis noget går galt under upload
                return StatusCode(500, $"Noget gik galt under upload: {ex.Message}");
            }
        }

        // GET: api/laering/fil/{id}
        // Returnerer filen for et læringsmateriale som en downloadbar fil
        [HttpGet("fil/{id}")]
        public async Task<IActionResult> HentFil(int id)
        {
            var læring = await _repo.GetByIdAsync(id);
            if (læring == null || string.IsNullOrEmpty(læring.FilIndholdBase64))
                return NotFound();

            var bytes = Convert.FromBase64String(læring.FilIndholdBase64);

            // Tilføjer Content-Disposition header, så filen downloades med korrekt navn
            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{læring.FilNavn}\"");

            // Returnerer filen med korrekt MIME-type
            return File(bytes, GetContentType(læring.Filtype));
        }

        // Hjælpefunktion til at bestemme korrekt MIME-type baseret på filtypen
        private string GetContentType(string filtype) => filtype.ToLower() switch
        {
            "pdf" => "application/pdf",
            "jpg" => "image/jpeg",
            "jpeg" => "image/jpeg",
            "png" => "image/png",
            "mp4" => "video/mp4",
            _ => "application/octet-stream"
        };
    }
}
