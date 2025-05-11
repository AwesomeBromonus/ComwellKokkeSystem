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

    [HttpGet("praktikperiode/{praktikperiodeId}")]
    public async Task<IActionResult> GetByPraktikperiodeId(int praktikperiodeId)
    {
        var result = await _repo.GetByPraktikperiodeIdAsync(praktikperiodeId);
        return Ok(result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDelmaal(int id, [FromBody] Modeller.Delmål delmaal)
    {
        if (delmaal.Id != id)
            return BadRequest("ID mismatch");

        await _repo.UpdateDelmaalAsync(delmaal);
        return Ok();
    }

}
