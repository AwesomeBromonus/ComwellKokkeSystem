using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller til håndtering af underdelmål *@
    [ApiController]
    [Route("api/underdelmaal")]
    public class UnderdelmaalController : ControllerBase
    {
        private readonly IUnderdelmaal _repository;

        // @* KONSTRUKTØR: Injicerer repository for databaseadgang *@
        public UnderdelmaalController(IUnderdelmaal repository)
        {
            _repository = repository;
        }

        // @* METODE: Hent alle underdelmål til et specifikt delmål *@
        [HttpGet("delmaal/{delmaalId}")]
        public async Task<IActionResult> GetByDelmaalId(int delmaalId)
        {
            var result = await _repository.GetByDelmaalIdAsync(delmaalId);
            return Ok(result);
        }

        // @* METODE: Hent enkelt underdelmål efter id *@
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var underdelmaal = await _repository.GetByIdAsync(id);
            if (underdelmaal == null)
                return NotFound();

            return Ok(underdelmaal);
        }

        // @* METODE: Tilføj nyt underdelmål *@
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Underdelmaal model)
        {
            await _repository.AddAsync(model);
            return Ok();
        }

        // @* METODE: Opdater eksisterende underdelmål; tjek id match *@
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Underdelmaal model)
        {
            if (id != model.Id)
                return BadRequest("Id stemmer ikke overens");

            await _repository.UpdateAsync(model);
            return Ok();
        }

        // @* METODE: Opdater kun statusfelt for underdelmål *@
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string nyStatus)
        {
            await _repository.UpdateStatusAsync(id, nyStatus);
            return Ok();
        }

        // @* METODE: Slet underdelmål baseret på id *@
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
