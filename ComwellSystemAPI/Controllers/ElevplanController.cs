using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Modeller;
using ComwellSystemAPI.Interfaces;

[ApiController]
[Route("api/elevplan")] // Definerer base-URL for controlleren
public class ElevplanController : ControllerBase
{
    private readonly IElevplan _repo;

    // Konstruktor hvor repository injiceres via dependency injection
    public ElevplanController(IElevplan repo)
    {
        _repo = repo;
    }

    // GET: api/elevplan
    // Henter alle elevplaner i systemet asynkront
    [HttpGet]
    public async Task<ActionResult<ActionResult<Modeller.Elevplan>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // GET: api/elevplan/{id}
    // Henter en enkelt elevplan ud fra dens unikke id
    [HttpGet("{id}")]
    public async Task<ActionResult<Modeller.Elevplan>> GetById(int id)
    {
        var plan = await _repo.GetByIdAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }



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
        return Ok(elevplan); // Returnerer den oprettede elevplan inkl. id
    }

    // PUT: api/elevplan/{id}
    // Opdaterer en eksisterende elevplan
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateElevplan(int id, [FromBody] Elevplan elevplan)
    {
        if (elevplan.Id != id)
            return BadRequest("ID mismatch"); // Sikrer at id i URL og body matcher

        await _repo.UpdateAsync(elevplan);
        return Ok();
    }

    // DELETE: api/elevplan/{id}
    // Sletter en elevplan baseret på id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent(); // Returnerer HTTP status 204 (No Content) ved succesfuld sletning
    }



}
