using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

[ApiController]
[Route("api/praktikperiode")] // Alle endpoints starter med /api/praktikperiode
public class PraktikperiodeController : ControllerBase
{
    private readonly IPraktikperiode _repo;

    public PraktikperiodeController(IPraktikperiode repo)
    {
        _repo = repo;
    }

    // GET: api/praktikperiode
    [HttpGet]
    public async Task<ActionResult<List<Praktikperiode>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // GET: api/praktikperiode/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Praktikperiode>> GetById(int id)
    {
        var periode = await _repo.GetByIdAsync(id);
        return periode == null ? NotFound() : Ok(periode);
    }

    // POST: api/praktikperiode
    [HttpPost]
    public async Task<IActionResult> CreatePraktikperiode([FromBody] Praktikperiode periode)
    {
        await _repo.AddAsync(periode);
        return Ok(periode);
    }

    // PUT: api/praktikperiode/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Praktikperiode periode)
    {
        if (id != periode.Id)
            return BadRequest("ID mismatch.");

        await _repo.UpdateAsync(periode);
        return NoContent();
    }

    // DELETE: api/praktikperiode/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }

    // ✅ GET: api/praktikperiode/elevplan/{elevplanId}
    [HttpGet("elevplan/{elevplanId}")]
    public async Task<ActionResult<List<Praktikperiode>>> GetByElevplanId(int elevplanId)
    {
        var praktikperioder = await _repo.GetByElevplanIdAsync(elevplanId);
        return Ok(praktikperioder);
    }
}
