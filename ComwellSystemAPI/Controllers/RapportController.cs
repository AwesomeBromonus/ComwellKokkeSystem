using Microsoft.AspNetCore.Mvc;
using ComwellSystemAPI.Interfaces;

namespace ComwellSystemAPI.Controllers
{
    [ApiController]
    [Route("api/rapport")]
    public class RapportController : ControllerBase
    {
        private readonly IRapportRepository _rapportRepo;

        public RapportController(IRapportRepository rapportRepo)
        {
            _rapportRepo = rapportRepo;
        }

        [HttpGet("excel")]
        public async Task<IActionResult> DownloadExcelRapport()
        {
            try
            {
                var excelData = await _rapportRepo.GenererElevDelmaalExcelAsync();

                return File(
                    excelData,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"Elevrapport_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ FEJL i Excel-generering:");
                Console.WriteLine(ex.ToString()); // <-- vigtigt
                return StatusCode(500, "Fejl: " + ex.Message);
            }
        }

    }
}
