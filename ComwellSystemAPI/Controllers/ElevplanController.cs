using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<IEnumerable<Elevplan>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // Henter en enkelt elevplan ud fra ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Elevplan>> GetById(int id)
    {
        var plan = await _repo.GetByIdAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }

    // Opretter en ny elevplan i databasen
    [HttpPost]
    public async Task<IActionResult> Create(Elevplan plan)
    {
        await _repo.AddAsync(plan);

        // Returnerer status 201 (Created) med henvisning til den nye ressource
        return CreatedAtAction(nameof(GetById), new { id = plan.Id }, plan);
    }

    // Opdaterer en eksisterende elevplan (baseret p� ID)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Elevplan plan)
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
}


