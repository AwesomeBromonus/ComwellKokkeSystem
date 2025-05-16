using Microsoft.AspNetCore.Mvc;
using Modeller;
using Interface;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/laering")]
    public class LæringController : ControllerBase
    {
        private readonly ILæring _repo;

        public LæringController(ILæring repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var laeringer = await _repo.GetAllAsync();
            return Ok(laeringer);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var læring = await _repo.GetByIdAsync(id);
            if (læring == null)
                return NotFound();

            return Ok(læring);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Læring læring)
        {
            if (læring == null)
                return BadRequest("Læring er null");

            await _repo.AddAsync(læring);
            return Ok(læring);
        }

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
