// ComwellSystemAPI/Controllers/RapportController.cs
using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Modeller;
using System.Collections.Generic; // Husk denne for List<T>

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

        [HttpGet("delmaal-with-underdelmaal/{year}")]
        public async Task<IActionResult> GetDelmaalWithUnderdelmaal(int year)
        {
            try
            {
                var delmaal = await _rapportService.GetAllDelmaalWithUnderdelmaalAsync(year);
                return Ok(delmaal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmaalWithUnderdelmaal API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("praktikperioder/{year}")]
        public async Task<IActionResult> GetPraktikperioder(int year)
        {
            try
            {
                var perioder = await _rapportService.GetPraktikPerioderAsync(year);
                return Ok(perioder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPraktikperioder API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("delmaal/{year}")]
        public async Task<IActionResult> GetDelmaal(int year)
        {
            try
            {
                var delmaal = await _rapportService.GetDelmålAsync(year);
                return Ok(delmaal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmaal API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("brugere/{year}")]
        public async Task<IActionResult> GetBrugere(int year)
        {
            try
            {
                var brugere = await _rapportService.GetBrugereAsync(year);
                return Ok(brugere);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBrugere API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // RETTET HER: Input parameter er nu List<RapportElevDelmålViewModel>
        [HttpPost("export/excel")]
        public async Task<IActionResult> ExportExcel([FromBody] List<Modeller.RapportElevDelmålViewModel> dataToExport)
        {
            try
            {
                if (dataToExport == null || !dataToExport.Any())
                {
                    // Du kan vælge at returnere en 204 No Content eller en 400 Bad Request
                    // 204 kan give mening hvis det er et gyldigt scenarie at eksportere tom data
                    // 400 hvis det indikerer et problem med forespørgslen
                    return NoContent(); // Eller StatusCode(400, "No data provided for export.");
                }

                var excelBytes = await _rapportService.ExportToExcelAsync(dataToExport);

                if (excelBytes == null || excelBytes.Length == 0)
                {
                    return StatusCode(500, "Failed to generate Excel file or file was empty.");
                }

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "rapport.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExportExcel API endpoint: {ex.Message}");
                // Inkluder StackTrace i udviklingsmiljøer for bedre debugging
                return StatusCode(500, $"Internal server error during Excel export: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // TILFØJET: Implementering af de "grå" metoder i Controlleren, hvis de skal bruges via API'en
        [HttpGet("delmaal-month/{year}/{month}")]
        public async Task<IActionResult> GetDelmålMåned(int year, int month)
        {
            try
            {
                var delmaal = await _rapportService.GetDelmålMånedAsync(year, month);
                return Ok(delmaal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDelmålMåned API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("fuldforte-delmaal/{year}")]
        public async Task<IActionResult> GetFuldførteDelmål(int year)
        {
            try
            {
                var delmaal = await _rapportService.GetFuldførteDelmålAsync(year);
                return Ok(delmaal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFuldførteDelmål API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("praktikperioder-per-elev/{elevId}/{year}")]
        public async Task<IActionResult> GetPraktikPerioderPerElev(int elevId, int year)
        {
            try
            {
                var perioder = await _rapportService.GetPraktikPerioderPerElevAsync(elevId, year);
                return Ok(perioder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPraktikPerioderPerElev API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("total-timer/{year}")]
        public async Task<IActionResult> GetTotalTimer(int year)
        {
            try
            {
                var totalTimer = await _rapportService.GetTotalTimerAsync(year);
                return Ok(totalTimer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalTimer API: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}