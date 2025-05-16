using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/kommentar")]
    public class KommentarController : ControllerBase
    {
        private readonly IKommentar _repo;

        public KommentarController(IKommentar repo)
        {
            _repo = repo;
        }

        // POST: api/kommentar
        // Tilføjer en ny kommentar til et bestemt delmål
        [HttpPost]
        public async Task<IActionResult> TilføjKommentar([FromBody] Kommentar kommentar)
        {
            if (kommentar == null || kommentar.DelmålId == 0 || string.IsNullOrWhiteSpace(kommentar.Indhold))
                return BadRequest("Ugyldig kommentar");

            await _repo.AddAsync(kommentar);
            return Ok();
        }

        // GET: api/kommentar/delmål/5
        // Henter alle kommentarer til et bestemt delmål
        [HttpGet("delmål/{delmålId}")]
        public async Task<IActionResult> HentKommentarer(int delmålId)
        {
            var kommentarer = await _repo.GetByDelmålIdAsync(delmålId);
            return Ok(kommentarer);
        }
    }
}
