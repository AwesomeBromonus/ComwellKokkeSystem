using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller til delmålsskabeloner, matcher kald fra Blazor Razor-siden *@
    [ApiController]
    [Route("api/delmaalskabelon")]
    public class DelmaalskabelonController : ControllerBase
    {
        private readonly IDelmaalSkabelon _repo;

        // @* KONSTRUKTØR: Injicerer repository til datatilgang *@
        public DelmaalskabelonController(IDelmaalSkabelon repo)
        {
            _repo = repo;
        }

        // @* METODE: Henter alle delmålsskabeloner via GET api/delmaalskabelon *@
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        // @* METODE: Henter delmålsskabeloner for en specifik praktikperiode via GET api/delmaalskabelon/periode/{nr} *@
        [HttpGet("periode/{nr}")]
        public async Task<IActionResult> GetByPeriode(int nr)
        {
            var result = await _repo.GetByPraktikperiodeNrAsync(nr);
            return result.Any() ? Ok(result) : NotFound();
        }

        // @* METODE: Henter en enkelt delmålsskabelon efter id via GET api/delmaalskabelon/{id} *@
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        // @* METODE: Tilføjer en ny delmålsskabelon via POST api/delmaalskabelon *@
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DelmaalSkabelon model)
        {
            await _repo.AddAsync(model);
            return Ok();
        }

        // @* METODE: Opdaterer en delmålsskabelon via PUT api/delmaalskabelon/{id}, tjekker id-match *@
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DelmaalSkabelon model)
        {
            if (id != model.Id) return BadRequest("ID mismatch");
            await _repo.UpdateAsync(model);
            return Ok();
        }

        // @* METODE: Sletter en delmålsskabelon efter id via DELETE api/delmaalskabelon/{id} *@
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
