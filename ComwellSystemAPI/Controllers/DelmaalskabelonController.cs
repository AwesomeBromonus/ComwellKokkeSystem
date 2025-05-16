using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/delmaalskabelon")] // <-- matcher Razor sidekald
    public class DelmaalskabelonController : ControllerBase

    {
        private readonly IDelmaalSkabelon _repo;

        public DelmaalskabelonController(IDelmaalSkabelon repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.GetAllAsync());

        [HttpGet("periode/{nr}")]
        public async Task<IActionResult> GetByPeriode(int nr)
        {
            var result = await _repo.GetByPraktikperiodeNrAsync(nr);
            return result.Any() ? Ok(result) : NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item != null ? Ok(item) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DelmaalSkabelon model)
        {
            await _repo.AddAsync(model);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DelmaalSkabelon model)
        {
            if (id != model.Id) return BadRequest("ID mismatch");
            await _repo.UpdateAsync(model);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}