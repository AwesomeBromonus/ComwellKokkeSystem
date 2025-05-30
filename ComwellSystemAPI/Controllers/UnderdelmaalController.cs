using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // API-controller til håndtering af underdelmål
    [ApiController]
    [Route("api/underdelmaal")]
    public class UnderdelmaalController : ControllerBase
    {
        private readonly IUnderdelmaal _repository;

        // Konstruktor hvor repository injiceres for databaseadgang
        public UnderdelmaalController(IUnderdelmaal repository)
        {
            _repository = repository;
        }

        // GET: api/underdelmaal/delmaal/{delmaalId}
        // Henter alle underdelmål tilknyttet et specifikt delmål
        [HttpGet("delmaal/{delmaalId}")]
        public async Task<IActionResult> GetByDelmaalId(int delmaalId)
        {
            var result = await _repository.GetByDelmaalIdAsync(delmaalId);
            return Ok(result);
        }

        // GET: api/underdelmaal/{id}
        // Henter et enkelt underdelmål baseret på id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var underdelmaal = await _repository.GetByIdAsync(id);
            if (underdelmaal == null)
                return NotFound();

            return Ok(underdelmaal);
        }

        // POST: api/underdelmaal
        // Tilføjer et nyt underdelmål
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Underdelmaal model)
        {
            await _repository.AddAsync(model);
            return Ok();
        }

        // PUT: api/underdelmaal/{id}
        // Opdaterer et eksisterende underdelmål; tjekker at id matcher
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Underdelmaal model)
        {
            if (id != model.Id)
                return BadRequest("Id stemmer ikke overens");

            await _repository.UpdateAsync(model);
            return Ok();
        }

        // PATCH: api/underdelmaal/status/{id}
        // Opdaterer kun statusfeltet for et underdelmål
        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string nyStatus)
        {
            await _repository.UpdateStatusAsync(id, nyStatus);
            return Ok();
        }

        // DELETE: api/underdelmaal/{id}
        // Sletter et underdelmål baseret på id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
