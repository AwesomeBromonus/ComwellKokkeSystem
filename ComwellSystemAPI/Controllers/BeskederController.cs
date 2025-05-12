using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/besked")]
public class BeskederController : ControllerBase
{
    private readonly IBesked _repo;

    public BeskederController(IBesked repo)
    {
        _repo = repo;
    }

    // Henter alle beskeder fra databasen
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Modeller.Besked>>> GetAll()
    {
        return Ok(await _repo.GetAllAsync());
    }

    // Henter en specifik besked fra databasen
    [HttpGet("{id}")]
    public async Task<ActionResult<Modeller.Besked>> GetById(int id)
    {
        var besked = await _repo.GetByIdAsync(id);
        if (besked == null)
            return NotFound($"Besked med ID {id} blev ikke fundet.");
        return Ok(besked);
    }

    // Opretter en ny besked i databasen
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Modeller.Besked besked)
    {
        await _repo.AddAsync(besked);
        return CreatedAtAction(nameof(GetById), new { id = besked.Id }, besked);
    }

    // Opdaterer en eksisterende besked
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Modeller.Besked besked)
    {
        if (id != besked.Id)
            return BadRequest("ID matcher ikke med den opdaterede besked.");

        var existingBesked = await _repo.GetByIdAsync(id);
        if (existingBesked == null)
            return NotFound($"Besked med ID {id} blev ikke fundet.");

        await _repo.UpdateAsync(besked);
        return NoContent();
    }

    // Sletter en besked fra databasen
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingBesked = await _repo.GetByIdAsync(id);
        if (existingBesked == null)
            return NotFound($"Besked med ID {id} blev ikke fundet.");

        await _repo.DeleteAsync(id);
        return NoContent();
    }
}


