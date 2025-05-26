// ComwellSystemAPI.Interfaces/IGenereRapport.cs
using Modeller; // Sørg for denne using er her
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // Vigtig for DateTime

namespace ComwellSystemAPI.Interfaces
{
    public interface IGenereRapport
    {
        Task<List<Praktikperiode>> GetPraktikPerioderAsync(int year);
        Task<List<Delmål>> GetDelmålAsync(int year);
        Task<List<UserModel>> GetBrugereAsync(int year);
        Task<byte[]> ExportToExcelAsync(List<RapportElevDelmålViewModel> dataToExport);

        // NYT: Denne metode henter alle delmål for et år, inklusive deres underdelmål
        Task<List<Delmål>> GetAllDelmaalWithUnderdelmaalAsync(int year);

        // TILFØJET: Disse metoder skal også være defineret i interfacet
        Task<List<Delmål>> GetDelmålMånedAsync(int year, int month);
        Task<List<Delmål>> GetFuldførteDelmålAsync(int year);
        Task<List<Praktikperiode>> GetPraktikPerioderPerElevAsync(int elevId, int year);
        Task<int> GetTotalTimerAsync(int year);
    }
}