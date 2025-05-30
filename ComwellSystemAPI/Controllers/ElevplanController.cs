using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Modeller;
using ComwellSystemAPI.Interfaces;

[ApiController]
[Route("api/elevplan")] // API-endpoint bliver: /api/elevplan
public class ElevplanController : ControllerBase
{
    private readonly IElevplan _repo;

    // Dependency injection af repository til databaseadgang
    public ElevplanController(IElevplan repo)
    {
        _repo = repo;
    }

    // Henter alle elevplaner fra databasen
    [HttpGet]
    public async Task<ActionResult<ActionResult<Modeller.Elevplan>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // Henter en enkelt elevplan ud fra ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Modeller.Elevplan>> GetById(int id)
    {
        var plan = await _repo.GetByIdAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }


  
    // Hent elevplan for en specifik elev
    [HttpGet("elev/{elevId}")]
    public async Task<ActionResult<Elevplan>> GetByElevId(int elevId)
    {
        var plan = await _repo.GetByElevIdAsync(elevId);
        if (plan == null)
            return NotFound("Der findes ingen elevplan for denne elev.");

        return Ok(plan);
    }


    // Opretter en ny elevplan i databasen
    [HttpPost]
    public async Task<IActionResult> CreateElevplan([FromBody] Elevplan elevplan)
    {
        await _repo.AddAsync(elevplan);
        return Ok(elevplan); // returnér oprettet plan med ID
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateElevplan(int id, [FromBody] Elevplan elevplan)
    {
        if (elevplan.Id != id) return BadRequest("ID mismatch");
        await _repo.UpdateAsync(elevplan);
        return Ok();
    }

    // Sletter en elevplan baseret p� ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent(); // Returnerer status 204 (No Content)
    }

}


