using Microsoft.AspNetCore.Mvc;
using Modeller;
using Interface;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller til håndtering af læringsmaterialer *@
    [ApiController]
    [Route("api/laering")]
    public class LæringController : ControllerBase
    {
        private readonly ILæring _repo;

        // @* KONSTRUKTØR: Injicerer repository til datatilgang *@
        public LæringController(ILæring repo)
        {
            _repo = repo;
        }

        // @* METODE: Henter alle læringsmaterialer via GET api/laering *@
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var laeringer = await _repo.GetAllAsync();
            return Ok(laeringer);
        }

        // @* METODE: Henter et enkelt læringsmateriale efter id via GET api/laering/{id} *@
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var læring = await _repo.GetByIdAsync(id);
            if (læring == null)
                return NotFound();

            return Ok(læring);
        }

        // @* METODE: Tilføjer nyt læringsmateriale via POST api/laering *@
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Læring læring)
        {
            if (læring == null)
                return BadRequest("Læring er null");

            await _repo.AddAsync(læring);
            return Ok(læring);
        }

        // @* METODE: Upload af fil med titel og beskrivelse via POST api/laering/upload *@
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string titel, [FromForm] string beskrivelse)
        {
            try
            {
                var tilladteTyper = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".mp4" };
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (file == null || file.Length == 0)
                    return BadRequest("Ingen fil valgt.");

                if (!tilladteTyper.Contains(extension))
                    return BadRequest("Filtypen er ikke tilladt.");

                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                var filBytes = ms.ToArray();
                var base64 = Convert.ToBase64String(filBytes);

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

                await _repo.AddAsync(læring);
                return Ok(læring);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Noget gik galt under upload: {ex.Message}");
            }
        }

        // @* METODE: Returnerer filindhold som downloadbar fil via GET api/laering/fil/{id} *@
        [HttpGet("fil/{id}")]
        public async Task<IActionResult> HentFil(int id)
        {
            var læring = await _repo.GetByIdAsync(id);
            if (læring == null || string.IsNullOrEmpty(læring.FilIndholdBase64))
                return NotFound();

            var bytes = Convert.FromBase64String(læring.FilIndholdBase64);

            Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{læring.FilNavn}\"");

            return File(bytes, GetContentType(læring.Filtype));
        }

        // @* HJÆLPEMETODE: Returnerer MIME-type baseret på filtypen *@
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
