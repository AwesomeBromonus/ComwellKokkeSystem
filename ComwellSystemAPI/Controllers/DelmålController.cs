using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

// API-controller der håndterer HTTP-forespørgsler relateret til delmål
[ApiController]
[Route("api/[controller]")]
public class DelmaalController : ControllerBase
{
    private readonly IDelmål _repo;

    // Konstruktor hvor repository injiceres til datatilgang
    public DelmaalController(IDelmål repo)
    {
        _repo = repo;
    }

    // POST: api/delmaal
    // Tilføjer et nyt delmål til databasen
    [HttpPost]
    public async Task<IActionResult> AddDelmaal([FromBody] Delmål delmaal)
    {
        await _repo.AddAsync(delmaal);
        return Ok();
    }

    // PUT: api/delmaal/{id}
    // Opdaterer et eksisterende delmål. Tjekker først at id i url og body stemmer overens
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDelmaal(int id, [FromBody] Delmål delmaal)
    {
        if (delmaal.Id != id)
            return BadRequest("ID mismatch");

        await _repo.UpdateDelmaalAsync(delmaal);
        return Ok();
    }

    // DELETE: api/delmaal/{id}
    // Sletter et delmål baseret på id, hvis det findes
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDelmaal(int id)
    {
        var eksisterende = await _repo.GetByIdAsync(id);
        if (eksisterende == null)
            return NotFound();

        await _repo.DeleteDelmaalAsync(id);
        return Ok();
    }

    // GET: api/delmaal/{id}
    // Henter et enkelt delmål baseret på id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var delmaal = await _repo.GetByIdAsync(id);
        if (delmaal == null)
            return NotFound();

        return Ok(delmaal);
    }

    // GET: api/delmaal/praktikperiode/{praktikperiodeId}
    // Henter alle delmål der tilhører en given praktikperiode
    [HttpGet("praktikperiode/{praktikperiodeId}")]
    public async Task<IActionResult> GetByPraktikperiodeId(int praktikperiodeId)
    {
        var result = await _repo.GetByPraktikperiodeIdAsync(praktikperiodeId);
        return Ok(result);
    }

    // GET: api/delmaal/elevplan/{elevplanId}/praktikperiode/{praktikperiodeId}
    // Henter delmål for en specifik elevplan og praktikperiode
    [HttpGet("elevplan/{elevplanId}/praktikperiode/{praktikperiodeId}")]
    public async Task<IActionResult> GetByElevplanAndPraktikperiode(int elevplanId, int praktikperiodeId)
    {
        var result = await _repo.GetByElevplanIdAndPraktikperiodeIdAsync(elevplanId, praktikperiodeId);
        return Ok(result);
    }

    // GET: api/delmaal/elev/{elevId}
    // Henter alle delmål tilknyttet en specifik elev
    [HttpGet("elev/{elevId}")]
    public async Task<IActionResult> GetByElevId(int elevId)
    {
        var result = await _repo.GetByElevIdAsync(elevId);
        return Ok(result);
    }

    // GET: api/delmaal/all
    // Henter alle delmål i systemet
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repo.GetAllAsync();
        return Ok(result);
    }

    // GET: api/delmaal/deadlines-14dage
    // Henter delmål med deadline inden for de næste 14 dage
    [HttpGet("deadlines-14dage")]
    public async Task<IActionResult> GetDelmaalMedDeadlineIndenFor14Dage()
    {
        var result = await _repo.GetWithDeadlineWithinDaysAsync(14);
        return Ok(result);
    }
}
