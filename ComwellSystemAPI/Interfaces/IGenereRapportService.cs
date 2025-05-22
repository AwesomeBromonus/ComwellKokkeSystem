// ComwellSystemAPI.Interfaces/IGenereRapport.cs
using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces
{
    public interface IGenereRapport
    {
        Task<List<Praktikperiode>> GetPraktikPerioderAsync(int year);
        Task<List<Delmål>> GetDelmålAsync(int year);
        Task<List<Delmål>> GetDelmålMånedAsync(int year, int month);
        Task<List<UserModel>> GetBrugereAsync(int year);
        Task<List<Delmål>> GetFuldførteDelmålAsync(int year);
        Task<List<Praktikperiode>> GetPraktikPerioderPerElevAsync(int elevId, int year);
        Task<int> GetTotalTimerAsync(int year);
        
        Task<Byte[]> ExportToCsvAsync(int year);
        Task<Byte[]> ExportToExcelAsync(int year);

        // NYT: Denne metode henter alle delmål for et år, inklusive deres underdelmål
        Task<List<Delmål>> GetAllDelmaalWithUnderdelmaalAsync(int year);
    }
}