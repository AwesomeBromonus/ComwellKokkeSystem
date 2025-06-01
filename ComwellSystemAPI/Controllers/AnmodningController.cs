using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // Markeret som API-controller og med route prefix "api/anmodning"
    [ApiController]
    [Route("api/anmodning")]
    public class AnmodningController : ControllerBase
    {
        private readonly IAnmodningRepository _repo;

        // Constructor, hvor repository injiceres for at kunne tilgå data
        public AnmodningController(IAnmodningRepository repo)
        {
            _repo = repo;
        }

        // POST: api/anmodning
        // Opretter en ny anmodning ud fra data sendt i body
        [HttpPost]
        public async Task<IActionResult> Opret([FromBody] Anmodning anmodning)
        {
            // Validerer at anmodningen indeholder nødvendige data
            if (anmodning == null || anmodning.DelmaalId <= 0 || anmodning.ElevId <= 0 || anmodning.ModtagerId <= 0)
                return BadRequest("Ugyldig anmodning.");

            // Kalder repository for at oprette anmodningen i databasen
            await _repo.OpretAsync(anmodning);
            return Ok("Anmodning oprettet.");
        }

        // GET: api/anmodning/{id}
        // Henter en anmodning baseret på dens id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var anmodning = await _repo.GetByIdAsync(id);
            if (anmodning == null)
                return NotFound("Anmodning ikke fundet.");

            return Ok(anmodning);
        }

        // GET: api/anmodning/modtager/{modtagerId}
        // Henter alle anmodninger, som er til en specifik modtager
        [HttpGet("modtager/{modtagerId}")]
        public async Task<IActionResult> GetTilModtager(int modtagerId)
        {
            var liste = await _repo.GetTilModtagerAsync(modtagerId);
            return Ok(liste);
        }

        // PUT: api/anmodning/{id}
        // Opdaterer en eksisterende anmodning med nye data
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

        // GET: api/anmodning
        // Henter alle anmodninger i systemet
        [HttpGet]
        public async Task<IActionResult> GetAlle()
        {
            var liste = await _repo.GetAlleAsync();
            return Ok(liste);
        }

        // PUT: api/anmodning/behandl/{id}
        // Ændrer status på en anmodning (f.eks. accepteret eller afvist)
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
