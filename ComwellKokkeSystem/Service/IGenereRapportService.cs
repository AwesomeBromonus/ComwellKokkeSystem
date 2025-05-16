using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service
{
    public interface IGenereRapportService
    {
        Task<List<Praktikperiode>?> GetPraktikPerioderAsync(int year);
        Task<List<DelmålDTO>?> GetDelmålAsync(int year);
        Task<List<UserModel>?> GetBrugereAsync(int year);
        Task<byte[]> ExportToCsvAsync(int year);
        Task<byte[]> ExportToExcelAsync(int year);

        Task<List<UserModel>> GetEleverAsync(int year); 
    }
}