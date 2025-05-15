using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces
{
    public interface IGenereRapport
    {
        Task<List<Praktikperiode>> GetPraktikPerioderAsync(int year);
        Task<List<DelmålDTO>> GetDelmålAsync(int year); // DelmålDTO
        Task<List<UserModel>> GetBrugereAsync(int year);
        Task<byte[]> ExportToCsvAsync(int year);
        Task<byte[]> ExportToExcelAsync(int year);
        Task<List<Praktikperiode>> GetPraktikperioderPerElevAsync(int elevId); 
        Task<List<Delmål>> GetFuldførteDelmålAsync(int elevId); 
    }
}