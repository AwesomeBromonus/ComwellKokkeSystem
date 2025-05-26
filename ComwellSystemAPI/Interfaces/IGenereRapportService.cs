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
        Task<List<UserModel>> GetBrugereAsync(int year);
        Task<Byte[]> ExportToExcelAsync(int year);

        // NYT: Denne metode henter alle delmål for et år, inklusive deres underdelmål
        Task<List<Delmål>> GetAllDelmaalWithUnderdelmaalAsync(int year);
    }
}