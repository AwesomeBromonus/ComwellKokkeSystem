
namespace ComwellKokkeSystem.Service
{ 
    public interface IRapportService
    {
        Task<byte[]> HentExcelRapportAsync();
    }
}
