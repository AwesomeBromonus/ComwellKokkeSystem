using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Modeller;
using ComwellSystemAPI.Interfaces;

[ApiController]
[Route("api/elevplan")] // Definerer base-URL for controlleren
public class ElevplanController : ControllerBase
{
    private readonly IElevplan _repo;

    // @* KONSTRUKTØR: Injicerer repository til datatilgang via dependency injection *@
    public ElevplanController(IElevplan repo)
    {
        _repo = repo;
    }

    // @* METODE: Henter alle elevplaner via GET api/elevplan *@
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Modeller.Elevplan>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // @* METODE: Henter enkelt elevplan efter id via GET api/elevplan/{id} *@
    [HttpGet("{id}")]
    public async Task<ActionResult<Modeller.Elevplan>> GetById(int id)
    {
        var plan = await _repo.GetByIdAsync(id);
        return plan == null ? NotFound() : Ok(plan);
    }

    // @* METODE: Henter alle elevplaner for en specifik elev via GET api/elevplan/elev/{elevId} *@
    [HttpGet("elev/{elevId}")]
    public async Task<ActionResult<List<Modeller.Elevplan>>> GetByElevId(int elevId)
    {
        var planer = await _repo.GetByElevIdAsync(elevId);
        if (planer == null || !planer.Any())
            return NotFound("Ingen elevplaner fundet for denne elev.");
        return Ok(planer);
    }

    // @* METODE: Opretter ny elevplan via POST api/elevplan *@
    [HttpPost]
    public async Task<IActionResult> CreateElevplan([FromBody] Elevplan elevplan)
    {
        await _repo.AddAsync(elevplan);
        return Ok(elevplan); // Returnerer oprettet plan inkl. id
    }

    // @* METODE: Opdaterer eksisterende elevplan via PUT api/elevplan/{id}, tjekker id-match *@
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateElevplan(int id, [FromBody] Elevplan elevplan)
    {
        if (elevplan.Id != id)
            return BadRequest("ID mismatch");

        await _repo.UpdateAsync(elevplan);
        return Ok();
    }

    // @* METODE: Sletter elevplan via DELETE api/elevplan/{id} *@
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent(); // HTTP 204 ved succes
    }

    // @* METODE: Henter nyeste elevplan for bruger via GET api/elevplan/byuser/{userId} *@
    [HttpGet("byuser/{userId}")]
    public async Task<IActionResult> GetLatestElevplanByUserId(int userId)
    {
        var planer = await _repo.GetByElevIdAsync(userId);
        var nyeste = planer?.OrderByDescending(p => p.OprettetDato).FirstOrDefault();

        return nyeste == null ? NotFound("Ingen elevplan fundet for brugeren.") : Ok(nyeste);
    }
}
