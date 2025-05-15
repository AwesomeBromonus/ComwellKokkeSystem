using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

[ApiController]
[Route("api/[controller]")]
public class DelmaalController : ControllerBase
{
    private readonly IDelmål _repo;

    public DelmaalController(IDelmål repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<IActionResult> AddDelmaal([FromBody] Delmål delmaal)
    {
        await _repo.AddAsync(delmaal);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDelmaal(int id, [FromBody] Delmål delmaal)
    {
        if (delmaal.Id != id)
            return BadRequest("ID mismatch");

        await _repo.UpdateDelmaalAsync(delmaal);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDelmaal(int id)
    {
        var eksisterende = await _repo.GetByIdAsync(id);
        if (eksisterende == null)
            return NotFound();

        await _repo.DeleteDelmaalAsync(id);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var delmaal = await _repo.GetByIdAsync(id);
        if (delmaal == null)
            return NotFound();

        return Ok(delmaal);
    }

    [HttpGet("praktikperiode/{praktikperiodeId}")]
    public async Task<IActionResult> GetByPraktikperiodeId(int praktikperiodeId)
    {
        var result = await _repo.GetByPraktikperiodeIdAsync(praktikperiodeId);
        return Ok(result);
    }

    [HttpGet("elevplan/{elevplanId}/praktikperiode/{praktikperiodeId}")]
    public async Task<IActionResult> GetByElevplanAndPraktikperiode(int elevplanId, int praktikperiodeId)
    {
        var result = await _repo.GetByElevplanIdAndPraktikperiodeIdAsync(elevplanId, praktikperiodeId);
        return Ok(result);
    }

    [HttpGet("elev/{elevId}")]
    public async Task<IActionResult> GetByElevId(int elevId)
    {
        var result = await _repo.GetByElevIdAsync(elevId);
        return Ok(result);
    }
}
