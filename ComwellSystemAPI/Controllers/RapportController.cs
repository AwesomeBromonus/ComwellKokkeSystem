using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RapportController : ControllerBase
    {
        private readonly IGenereRapport _rapportService;

        public RapportController(IGenereRapport rapportService)
        {
            _rapportService = rapportService;
        }

        [HttpGet("praktikperioder/{year}")]
        public async Task<IActionResult> GetPraktikperioder(int year)
        {
            var perioder = await _rapportService.GetPraktikPerioderAsync(year);
            return Ok(perioder);
        }

        [HttpGet("delmaal/{year}")]
        public async Task<IActionResult> GetDelmaal(int year)
        {
            var delmaal = await _rapportService.GetDelmålAsync(year);
            return Ok(delmaal);
        }

        [HttpGet("brugere/{year}")]
        public async Task<IActionResult> GetBrugere(int year)
        {
            var brugere = await _rapportService.GetBrugereAsync(year);
            return Ok(brugere);
        }

        [HttpGet("export/csv/{year}")]
        public async Task<IActionResult> ExportToCsv(int year)
        {
            try
            {
                var fileContent = await _rapportService.ExportToCsvAsync(year);
                return File(fileContent, "text/csv", $"rapport_{year}.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fejl ved eksport til CSV: {ex.Message}");
            }
        }

        [HttpGet("export/excel/{year}")]
        public async Task<IActionResult> ExportToExcel(int year)
        {
            try
            {
                var fileContent = await _rapportService.ExportToExcelAsync(year);
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"rapport_{year}.xlsx");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fejl ved eksport til Excel: {ex.Message}");
            }
        }
        
    }
}