using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    // API-controller med route "api/delmaalskabelon", matcher kald fra Blazor Razor-siden
    [ApiController]
    [Route("api/delmaalskabelon")]
    public class DelmaalskabelonController : ControllerBase
    {
        private readonly IDelmaalSkabelon _repo;

        // Constructor hvor repository til skabeloner injiceres for datatilgang
        public DelmaalskabelonController(IDelmaalSkabelon repo)
        {
            _repo = repo;
        }

        // GET: api/delmaalskabelon
        // Henter alle delmålsskabeloner
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        // GET: api/delmaalskabelon/periode/{nr}
        // Henter delmålsskabeloner for en specifik praktikperiode
        [HttpGet("periode/{nr}")]
        public async Task<IActionResult> GetByPeriode(int nr)
        {
            var result = await _repo.GetByPraktikperiodeNrAsync(nr);
            return result.Any() ? Ok(result) : NotFound();
        }

        // GET: api/delmaalskabelon/{id}
        // Henter en enkelt delmålsskabelon baseret på id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        // POST: api/delmaalskabelon
        // Tilføjer en ny delmålsskabelon
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DelmaalSkabelon model)
        {
            await _repo.AddAsync(model);
            return Ok();
        }

        // PUT: api/delmaalskabelon/{id}
        // Opdaterer en eksisterende delmålsskabelon
        // Sikrer at id i url og model stemmer overens
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DelmaalSkabelon model)
        {
            if (id != model.Id) return BadRequest("ID mismatch");
            await _repo.UpdateAsync(model);
            return Ok();
        }

        // DELETE: api/delmaalskabelon/{id}
        // Sletter en delmålsskabelon baseret på id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
