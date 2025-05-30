using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // API-controller til håndtering af underdelmålsskabeloner
    [ApiController]
    [Route("api/underdelmaalskabelon")]
    public class UnderdelmaalSkabelonController : ControllerBase
    {
        private readonly IUnderdelmaalSkabelon _repository;

        // Konstruktor hvor repository injiceres til at håndtere databaseoperationer
        public UnderdelmaalSkabelonController(IUnderdelmaalSkabelon repository)
        {
            _repository = repository;
        }

        // GET: api/underdelmaalskabelon/delmaalskabelon/{delmaalSkabelonId}
        // Henter alle underdelmålsskabeloner tilknyttet et bestemt delmålsskabelon-id
        [HttpGet("delmaalskabelon/{delmaalSkabelonId}")]
        public async Task<IActionResult> GetByDelmaalSkabelonId(int delmaalSkabelonId)
        {
            var result = await _repository.GetByDelmaalSkabelonIdAsync(delmaalSkabelonId);
            return Ok(result);
        }

        // POST: api/underdelmaalskabelon
        // Tilføjer en ny underdelmålsskabelon
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UnderdelmaalSkabelon model)
        {
            if (model == null)
                return BadRequest("Model er tom");

            await _repository.AddAsync(model);
            return Ok();
        }

        // DELETE: api/underdelmaalskabelon/{id}
        // Sletter en underdelmålsskabelon baseret på id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }

        // PUT: api/underdelmaalskabelon/{id}
        // Opdaterer en eksisterende underdelmålsskabelon
        // Sikrer at id i URL og i model stemmer overens
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UnderdelmaalSkabelon model)
        {
            if (id != model.Id)
                return BadRequest("ID i URL matcher ikke modellen");

            await _repository.UpdateAsync(model);
            return Ok();
        }
    }
}
