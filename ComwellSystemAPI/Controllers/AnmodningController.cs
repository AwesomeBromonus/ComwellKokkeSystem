using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/anmodning")]
    public class AnmodningController : ControllerBase
    {
        private readonly IAnmodningRepository _repo;

        public AnmodningController(IAnmodningRepository repo)
        {
            _repo = repo;
        }

        // POST: api/anmodning
        [HttpPost]
        public async Task<IActionResult> Opret([FromBody] Anmodning anmodning)
        {
            if (anmodning == null || anmodning.DelmaalId <= 0 || anmodning.ElevId <= 0 || anmodning.ModtagerId <= 0)
                return BadRequest("Ugyldig anmodning.");

            await _repo.OpretAsync(anmodning);
            return Ok("Anmodning oprettet.");
        }

        // GET: api/anmodning/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var anmodning = await _repo.GetByIdAsync(id);
            if (anmodning == null)
                return NotFound("Anmodning ikke fundet.");

            return Ok(anmodning);
        }

        // GET: api/anmodning/modtager/5
        [HttpGet("modtager/{modtagerId}")]
        public async Task<IActionResult> GetTilModtager(int modtagerId)
        {
            var liste = await _repo.GetTilModtagerAsync(modtagerId);
            return Ok(liste);
        }

        // PUT: api/anmodning/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Opdater(int id, [FromBody] Anmodning opdateret)
        {
            var eksisterende = await _repo.GetByIdAsync(id);
            if (eksisterende == null)
                return NotFound("Anmodning ikke fundet.");

            opdateret.Id = id; // sikre at ID forbliver korrekt
            await _repo.UpdateAsync(opdateret);

            return Ok("Anmodning opdateret.");
        }

        // GET: api/anmodning
        [HttpGet]
        public async Task<IActionResult> GetAlle()
        {
            var liste = await _repo.GetAlleAsync();
            return Ok(liste);
        }
        [HttpPut("behandl/{id}")]
        public async Task<IActionResult> Behandl(int id, [FromBody] bool accepteret)
        {
            var anmodning = await _repo.GetByIdAsync(id);
            if (anmodning == null)
                return NotFound();

            await _repo.BehandlAsync(id, accepteret); // ✅ Kald korrekt logik
            return Ok();
        }



    }
}
