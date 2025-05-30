using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller for anmodninger, håndterer HTTP-requests relateret til anmodning *@
    [ApiController]
    [Route("api/anmodning")]
    public class AnmodningController : ControllerBase
    {
        private readonly IAnmodningRepository _repo;

        // @* KONSTRUKTØR: Injicerer repository til datatilgang *@
        public AnmodningController(IAnmodningRepository repo)
        {
            _repo = repo;
        }

        // @* METODE: Opretter en ny anmodning via POST api/anmodning *@
        [HttpPost]
        public async Task<IActionResult> Opret([FromBody] Anmodning anmodning)
        {
            if (anmodning == null || anmodning.DelmaalId <= 0 || anmodning.ElevId <= 0 || anmodning.ModtagerId <= 0)
                return BadRequest("Ugyldig anmodning.");

            await _repo.OpretAsync(anmodning);
            return Ok("Anmodning oprettet.");
        }

        // @* METODE: Henter en anmodning baseret på id via GET api/anmodning/{id} *@
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var anmodning = await _repo.GetByIdAsync(id);
            if (anmodning == null)
                return NotFound("Anmodning ikke fundet.");

            return Ok(anmodning);
        }

        // @* METODE: Henter anmodninger til en bestemt modtager via GET api/anmodning/modtager/{modtagerId} *@
        [HttpGet("modtager/{modtagerId}")]
        public async Task<IActionResult> GetTilModtager(int modtagerId)
        {
            var liste = await _repo.GetTilModtagerAsync(modtagerId);
            return Ok(liste);
        }

        // @* METODE: Opdaterer en anmodning via PUT api/anmodning/{id} *@
        [HttpPut("{id}")]
        public async Task<IActionResult> Opdater(int id, [FromBody] Anmodning opdateret)
        {
            var eksisterende = await _repo.GetByIdAsync(id);
            if (eksisterende == null)
                return NotFound("Anmodning ikke fundet.");

            opdateret.Id = id; // Sikrer at id’et ikke ændres
            await _repo.UpdateAsync(opdateret);

            return Ok("Anmodning opdateret.");
        }

        // @* METODE: Henter alle anmodninger via GET api/anmodning *@
        [HttpGet]
        public async Task<IActionResult> GetAlle()
        {
            var liste = await _repo.GetAlleAsync();
            return Ok(liste);
        }

        // @* METODE: Ændrer status på en anmodning via PUT api/anmodning/behandl/{id} *@
        [HttpPut("behandl/{id}")]
        public async Task<IActionResult> Behandl(int id, [FromBody] bool accepteret)
        {
            var anmodning = await _repo.GetByIdAsync(id);
            if (anmodning == null)
                return NotFound();

            await _repo.BehandlAsync(id, accepteret);
            return Ok();
        }
    }
}
