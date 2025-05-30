using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller til håndtering af kommentarer relateret til delmål *@
    [ApiController]
    [Route("api/kommentar")]
    public class KommentarController : ControllerBase
    {
        private readonly IKommentar _repo;

        // @* KONSTRUKTØR: Injicerer repository til datatilgang via dependency injection *@
        public KommentarController(IKommentar repo)
        {
            _repo = repo;
        }

        // @* METODE: Tilføjer en ny kommentar til et delmål via POST api/kommentar *@
        [HttpPost]
        public async Task<IActionResult> TilføjKommentar([FromBody] Kommentar kommentar)
        {
            // Validering af kommentarindhold og tilknytning
            if (kommentar == null || kommentar.DelmålId == 0 || string.IsNullOrWhiteSpace(kommentar.Indhold))
                return BadRequest("Ugyldig kommentar");

            await _repo.AddAsync(kommentar);
            return Ok();
        }

        // @* METODE: Henter alle kommentarer for et delmål via GET api/kommentar/delmål/{delmålId} *@
        [HttpGet("delmål/{delmålId}")]
        public async Task<IActionResult> HentKommentarer(int delmålId)
        {
            var kommentarer = await _repo.GetByDelmålIdAsync(delmålId);
            return Ok(kommentarer);
        }
    }
}
