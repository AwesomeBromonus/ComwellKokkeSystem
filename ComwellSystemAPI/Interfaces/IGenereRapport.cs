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
        Task<List<Delmål>> GetFuldførteDelmålAsync(int year); // Nyttig for statusrapporter
        Task<List<Praktikperiode>> GetPraktikPerioderPerElevAsync(int elevId, int year); // Rapporter per elev
        Task<int> GetTotalTimerAsync(int year); // Hvis timer bliver implementeret senere
        
        Task<Byte[]> ExportToCsvAsync(int year);
        Task<Byte[]> ExportToExcelAsync(int year);
    }
}