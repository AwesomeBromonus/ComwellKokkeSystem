using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/underdelmaal")]
    public class UnderdelmaalController : ControllerBase
    {
        private readonly IUnderdelmaal _repository;

        public UnderdelmaalController(IUnderdelmaal repository)
        {
            _repository = repository;
        }

        [HttpGet("delmaal/{delmaalId}")]
        public async Task<IActionResult> GetByDelmaalId(int delmaalId)
        {
            var result = await _repository.GetByDelmaalIdAsync(delmaalId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var underdelmaal = await _repository.GetByIdAsync(id);
            if (underdelmaal == null)
                return NotFound();

            return Ok(underdelmaal);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Underdelmaal model)
        {
            await _repository.AddAsync(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Underdelmaal model)
        {
            if (id != model.Id)
                return BadRequest("Id stemmer ikke overens");

            await _repository.UpdateAsync(model);
            return Ok();
        }

        [HttpPatch("status/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string nyStatus)
        {
            await _repository.UpdateStatusAsync(id, nyStatus);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
