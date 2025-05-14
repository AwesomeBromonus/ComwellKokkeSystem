using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

[ApiController]
[Route("api/praktikperiode")] // Alle endpoints starter med /api/praktikperiode
public class PraktikperiodeController : ControllerBase
{
    private readonly IPraktikperiode _repo;

    // Constructor med dependency injection af repository
    public PraktikperiodeController(IPraktikperiode repo)
    {
        _repo = repo;
    }

    // GET: api/praktikperiode
    // Henter alle praktikperioder
    [HttpGet]
    public async Task<ActionResult<List<Praktikperiode>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // GET: api/praktikperiode/{id}
    // Henter én praktikperiode ud fra ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Praktikperiode>> GetById(int id)
    {
        var periode = await _repo.GetByIdAsync(id);
        return periode == null ? NotFound() : Ok(periode);
    }

    // POST: api/praktikperiode
    // Opretter en ny praktikperiode
    [HttpPost]
    public async Task<IActionResult> CreatePraktikperiode([FromBody] Praktikperiode periode)
    {
        await _repo.AddAsync(periode);
        return Ok(periode); // Returnér den nyoprettede periode
    }

    // PUT: api/praktikperiode/{id}
    // Opdaterer en praktikperiode med givet ID
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Praktikperiode periode)
    {
        if (id != periode.Id)
            return BadRequest("ID mismatch."); // Sikrer at URL og body matcher

        await _repo.UpdateAsync(periode);
        return NoContent(); // 204
    }

    // DELETE: api/praktikperiode/{id}
    // Sletter en praktikperiode ud fra ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent(); // 204
    }
}
