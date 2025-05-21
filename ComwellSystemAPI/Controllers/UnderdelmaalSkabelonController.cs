using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/underdelmaalskabelon")]
    public class UnderdelmaalSkabelonController : ControllerBase
    {
        private readonly IUnderdelmaalSkabelon _repository;

        public UnderdelmaalSkabelonController(IUnderdelmaalSkabelon repository)
        {
            _repository = repository;
        }

        // Hent alle underdelmålsskabeloner til et bestemt delmålsskabelon-id
        [HttpGet("delmaalskabelon/{delmaalSkabelonId}")]
        public async Task<IActionResult> GetByDelmaalSkabelonId(int delmaalSkabelonId)
        {
            var result = await _repository.GetByDelmaalSkabelonIdAsync(delmaalSkabelonId);
            return Ok(result);
        }

        // Tilføj ny underdelmålsskabelon
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UnderdelmaalSkabelon model)
        {
            if (model == null)
                return BadRequest("Model er tom");

            await _repository.AddAsync(model);
            return Ok();
        }

        // Slet en underdelmålsskabelon
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
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
