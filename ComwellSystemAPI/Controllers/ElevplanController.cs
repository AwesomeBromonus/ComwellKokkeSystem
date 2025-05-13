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
    public async Task<ActionResult<IEnumerable<Modeller.Elevplan>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // Henter en enkelt elevplan ud fra ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Modeller.Elevplan>> GetById(int id)
    {
        var plan = await _repo.GetByIdAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }


    // ✅ NYT ENDPOINT: Hent elevplaner for en specifik elev
    [HttpGet("elev/{elevId}")]
    public async Task<ActionResult<List<Modeller.Elevplan>>> GetByElevId(int elevId)
    {
        var planer = await _repo.GetByElevIdAsync(elevId);
        if (planer == null || !planer.Any()) return NotFound("Ingen elevplaner fundet for denne elev.");
        return Ok(planer);
    }

    // Opretter en ny elevplan i databasen
    [HttpPost]
    public async Task<IActionResult> Create(Modeller.Elevplan plan)
    {
        await _repo.AddAsync(plan);

        // Returnerer status 201 (Created) med henvisning til den nye ressource
        return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
    }

    // Opdaterer en eksisterende elevplan (baseret p� ID)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Modeller.Elevplan plan)
    {
        // Sikrer at ID i URL og objekt matcher
        if (id != plan.Id) return BadRequest("ID i URL og objekt matcher ikke.");

        await _repo.UpdateAsync(plan);
        return NoContent(); // Returnerer status 204 (No Content)
    }

    // Sletter en elevplan baseret p� ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent(); // Returnerer status 204 (No Content)
    }

    [HttpGet("byuser/{userId}")]
    public async Task<IActionResult> GetLatestElevplanByUserId(int userId)
    {
        var planer = await _repo.GetByElevIdAsync(userId);
        var nyeste = planer?.OrderByDescending(p => p.OprettetDato).FirstOrDefault();

        return nyeste == null ? NotFound("Ingen elevplan fundet for brugeren.") : Ok(nyeste);
    }



}


