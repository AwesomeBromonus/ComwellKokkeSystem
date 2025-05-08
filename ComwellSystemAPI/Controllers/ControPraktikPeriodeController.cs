using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

[ApiController]
[Route("api/praktikperiode")]
public class PraktikperiodeController : ControllerBase
{
    private readonly IPraktikperiode _repo;

    public PraktikperiodeController(IPraktikperiode repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<Praktikperiode>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Praktikperiode>> GetById(int id)
    {
        var periode = await _repo.GetByIdAsync(id);
        return periode == null ? NotFound() : Ok(periode);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Praktikperiode periode)
    {
        await _repo.AddAsync(periode);
        return CreatedAtAction(nameof(GetById), new { id = periode.Id }, periode);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Praktikperiode periode)
    {
        if (id != periode.Id)
            return BadRequest("ID mismatch.");

        await _repo.UpdateAsync(periode);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
