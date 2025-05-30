using Microsoft.AspNetCore.Mvc;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    // @* KLASSE: API-controller til rapportfunktioner *@
    [ApiController]
    [Route("api/rapport")]
    public class RapportController : ControllerBase
    {
        private readonly IRapportRepository _rapportRepo;

        // @* KONSTRUKTØR: Injicerer repository til rapportgenerering *@
        public RapportController(IRapportRepository rapportRepo)
        {
            _rapportRepo = rapportRepo;
        }

        // @* METODE: Endpoint til at hente Excel-rapport over elev delmål *@
        [HttpGet("excel")]
        public async Task<IActionResult> DownloadExcelRapport()
        {
            try
            {
                // Kalder repository til at generere Excel-data som byte-array
                var excelData = await _rapportRepo.GenererElevDelmaalExcelAsync();

                // Returnerer Excel-filen med korrekt MIME-type og dynamisk filnavn baseret på tid
                return File(
                    excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Elevrapport_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                // Logger fejl og returnerer HTTP 500 med fejlbesked
                Console.WriteLine("❌ FEJL i Excel-generering:");
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Fejl: " + ex.Message);
            }
        }
    }
}
