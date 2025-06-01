using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;

// @* KLASSE: API-controller der håndterer HTTP-forespørgsler relateret til delmål *@
[ApiController]
[Route("api/[controller]")]
public class DelmaalController : ControllerBase
{
    private readonly IDelmål _repo;

    // @* KONSTRUKTØR: Injicerer repository til datatilgang *@
    public DelmaalController(IDelmål repo)
    {
        _repo = repo;
    }

    // @* METODE: Tilføjer et nyt delmål via POST api/delmaal *@
    [HttpPost]
    public async Task<IActionResult> AddDelmaal([FromBody] Delmål delmaal)
    {
        await _repo.AddAsync(delmaal);
        return Ok();
    }

    // @* METODE: Opdaterer et delmål via PUT api/delmaal/{id}, tjekker id-match *@
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDelmaal(int id, [FromBody] Delmål delmaal)
    {
        if (delmaal.Id != id)
            return BadRequest("ID mismatch");

        await _repo.UpdateDelmaalAsync(delmaal);
        return Ok();
    }

    // @* METODE: Sletter et delmål via DELETE api/delmaal/{id} hvis findes *@
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDelmaal(int id)
    {
        var eksisterende = await _repo.GetByIdAsync(id);
        if (eksisterende == null)
            return NotFound();

        await _repo.DeleteDelmaalAsync(id);
        return Ok();
    }

    // @* METODE: Henter delmål efter id via GET api/delmaal/{id} *@
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var delmaal = await _repo.GetByIdAsync(id);
        if (delmaal == null)
            return NotFound();

        return Ok(delmaal);
    }

    // @* METODE: Henter delmål for praktikperiode via GET api/delmaal/praktikperiode/{praktikperiodeId} *@
    [HttpGet("praktikperiode/{praktikperiodeId}")]
    public async Task<IActionResult> GetByPraktikperiodeId(int praktikperiodeId)
    {
        var result = await _repo.GetByPraktikperiodeIdAsync(praktikperiodeId);
        return Ok(result);
    }


    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repo.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("deadlines-14dage")]
    public async Task<IActionResult> GetDelmaalMedDeadlineIndenFor14Dage()
    {
        var result = await _repo.GetWithDeadlineWithinDaysAsync(14);
        return Ok(result);
    }
}
