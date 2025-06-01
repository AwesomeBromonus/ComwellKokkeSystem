using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

[ApiController]
[Route("api/praktikperiode")] // Base URL for controllerens endpoints
public class PraktikperiodeController : ControllerBase
{
    private readonly IPraktikperiode _repo;

    // Konstruktor hvor repository injiceres for at give adgang til databasehandlinger
    public PraktikperiodeController(IPraktikperiode repo)
    {
        _repo = repo;
    }

    // GET: api/praktikperiode
    // Henter alle praktikperioder i systemet som en liste
    [HttpGet]
    public async Task<ActionResult<List<Praktikperiode>>> GetAll() =>
        Ok(await _repo.GetAllAsync());

    // GET: api/praktikperiode/{id}
    // Henter en enkelt praktikperiode baseret på dens unikke id
    [HttpGet("{id}")]
    public async Task<ActionResult<Praktikperiode>> GetById(int id)
    {
        var periode = await _repo.GetByIdAsync(id);
        return periode == null ? NotFound() : Ok(periode);
    }

    // POST: api/praktikperiode
    // Opretter en ny praktikperiode ud fra data modtaget i request body
    [HttpPost]
    public async Task<IActionResult> CreatePraktikperiode([FromBody] Praktikperiode periode)
    {
        await _repo.AddAsync(periode);
        return Ok(periode); // Returnerer den oprettede praktikperiode inkl. ID
    }

    // PUT: api/praktikperiode/{id}
    // Opdaterer en eksisterende praktikperiode; sikrer id-matching for datakonsistens
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Praktikperiode periode)
    {
        if (id != periode.Id)
            return BadRequest("ID mismatch.");

        await _repo.UpdateAsync(periode);
        return NoContent(); // Status 204, ingen data returneres
    }

    // DELETE: api/praktikperiode/{id}
    // Sletter en praktikperiode baseret på id
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent(); // Status 204 ved succesfuld sletning
    }

   
    // ✅ GET: api/praktikperiode/elevplan/{elevplanId}
    [HttpGet("elevplan/{elevplanId}")]
    public async Task<ActionResult<List<Praktikperiode>>> GetByElevplanId(int elevplanId)
    {
        var praktikperioder = await _repo.GetByElevplanIdAsync(elevplanId);
        return Ok(praktikperioder);
    }
}
