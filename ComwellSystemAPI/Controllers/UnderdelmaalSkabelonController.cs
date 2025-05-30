using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller til håndtering af underdelmålsskabeloner *@
    [ApiController]
    [Route("api/underdelmaalskabelon")]
    public class UnderdelmaalSkabelonController : ControllerBase
    {
        private readonly IUnderdelmaalSkabelon _repository;

        // @* KONSTRUKTØR: Injicerer repository til databaseoperationer *@
        public UnderdelmaalSkabelonController(IUnderdelmaalSkabelon repository)
        {
            _repository = repository;
        }

        // @* METODE: Hent alle underdelmålsskabeloner tilknyttet et delmålsskabelon-id *@
        [HttpGet("delmaalskabelon/{delmaalSkabelonId}")]
        public async Task<IActionResult> GetByDelmaalSkabelonId(int delmaalSkabelonId)
        {
            var result = await _repository.GetByDelmaalSkabelonIdAsync(delmaalSkabelonId);
            return Ok(result);
        }

        // @* METODE: Tilføj ny underdelmålsskabelon *@
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UnderdelmaalSkabelon model)
        {
            if (model == null)
                return BadRequest("Model er tom");

            await _repository.AddAsync(model);
            return Ok();
        }

        // @* METODE: Slet underdelmålsskabelon baseret på id *@
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }

        // @* METODE: Opdater eksisterende underdelmålsskabelon; sikrer id-match *@
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
