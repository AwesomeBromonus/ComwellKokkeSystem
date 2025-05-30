using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    // API-controller til håndtering af kommentarer relateret til delmål
    [ApiController]
    [Route("api/kommentar")]
    public class KommentarController : ControllerBase
    {
        private readonly IKommentar _repo;

        // Konstruktor hvor repository injiceres via dependency injection
        public KommentarController(IKommentar repo)
        {
            _repo = repo;
        }

        // POST: api/kommentar
        // Tilføjer en ny kommentar til et specifikt delmål
        [HttpPost]
        public async Task<IActionResult> TilføjKommentar([FromBody] Kommentar kommentar)
        {
            // Validerer at kommentar ikke er tom og tilknytning til delmål er korrekt
            if (kommentar == null || kommentar.DelmålId == 0 || string.IsNullOrWhiteSpace(kommentar.Indhold))
                return BadRequest("Ugyldig kommentar");

            // Gemmer kommentaren via repository
            await _repo.AddAsync(kommentar);
            return Ok();
        }

        // GET: api/kommentar/delmål/{delmålId}
        // Henter alle kommentarer knyttet til et specifikt delmål
        [HttpGet("delmål/{delmålId}")]
        public async Task<IActionResult> HentKommentarer(int delmålId)
        {
            var kommentarer = await _repo.GetByDelmålIdAsync(delmålId);
            return Ok(kommentarer);
        }
    }
}
