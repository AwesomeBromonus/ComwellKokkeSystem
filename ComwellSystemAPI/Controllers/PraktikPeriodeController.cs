using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

[ApiController]
[Route("api/praktikperiode")] // Base URL for controllerens endpoints
public class PraktikperiodeController : ControllerBase
{
    private readonly IPraktikperiode _repo;

    // @* KONSTRUKTØR: Injicerer repository til databasehandlinger *@
    public PraktikperiodeController(IPraktikperiode repo)
    {
        _repo = repo;
    }

    // @* METODE: Henter alle praktikperioder via GET api/praktikperiode *@
    [HttpGet]
    public async Task<ActionResult<List<Praktikperiode>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // @* METODE: Henter en enkelt praktikperiode efter id via GET api/praktikperiode/{id} *@
    [HttpGet("{id}")]
    public async Task<ActionResult<Praktikperiode>> GetById(int id)
    {
        var periode = await _repo.GetByIdAsync(id);
        return periode == null ? NotFound() : Ok(periode);
    }

    // @* METODE: Opretter en ny praktikperiode via POST api/praktikperiode *@
    [HttpPost]
    public async Task<IActionResult> CreatePraktikperiode([FromBody] Praktikperiode periode)
    {
        await _repo.AddAsync(periode);
        return Ok(periode); // Returnerer oprettet objekt inkl. id
    }

    // @* METODE: Opdaterer eksisterende praktikperiode via PUT api/praktikperiode/{id} *@
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Praktikperiode periode)
    {
        if (id != periode.Id)
            return BadRequest("ID mismatch.");

        await _repo.UpdateAsync(periode);
        return NoContent();
    }

    // @* METODE: Sletter praktikperiode via DELETE api/praktikperiode/{id} *@
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }

    // @* METODE: Henter praktikperioder for en elev via GET api/praktikperiode/elev/{elevId} *@
    [HttpGet("elev/{elevId}")]
    public async Task<ActionResult<List<Praktikperiode>>> GetPraktikperioderForElev(int elevId)
    {
        var praktikperioder = await _repo.GetByElevIdAsync(elevId);
        return Ok(praktikperioder);
    }

    // @* METODE: Henter praktikperioder for en elevplan via GET api/praktikperiode/elevplan/{elevplanId} *@
    [HttpGet("elevplan/{elevplanId}")]
    public async Task<ActionResult<List<Praktikperiode>>> GetByElevplanId(int elevplanId)
    {
        var praktikperioder = await _repo.GetByElevplanIdAsync(elevplanId);
        return Ok(praktikperioder);
    }
}
