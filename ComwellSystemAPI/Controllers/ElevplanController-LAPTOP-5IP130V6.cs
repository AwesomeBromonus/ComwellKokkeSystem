using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/elevplan")]
    public class ElevplanController : ControllerBase
    {
        private readonly IElevplan _repo;

        public ElevplanController(IElevplan repo)
        {
            _repo = repo;
        }

        // GET: api/elevplan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Elevplan>>> GetAll()
        {
            var planer = await _repo.GetAllAsync();
            return Ok(planer);
        }

        // GET: api/elevplan/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Elevplan>> GetById(int id)
        {
            var plan = await _repo.GetByIdAsync(id);
            return plan == null ? NotFound() : Ok(plan);
        }

        // GET: api/elevplan/elev/{elevId}
        [HttpGet("elev/{elevId}")]
        public async Task<ActionResult<List<Elevplan>>> GetByElevId(int elevId)
        {
            var planer = await _repo.GetByElevIdAsync(elevId);
            if (planer == null || !planer.Any())
                return NotFound("Ingen elevplaner fundet for denne elev.");

            return Ok(planer);
        }

        // POST: api/elevplan
        [HttpPost]
        public async Task<IActionResult> Create(Elevplan plan)
        {
            await _repo.AddAsync(plan);
            return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
        }

        // PUT: api/elevplan/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Elevplan plan)
        {
            if (id != plan.Id)
                return BadRequest("ID i URL og objekt matcher ikke.");

            await _repo.UpdateAsync(plan);
            return NoContent();
        }

        // DELETE: api/elevplan/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
