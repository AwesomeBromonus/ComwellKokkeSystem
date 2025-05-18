using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IDelmaalSkabelon
    {
        Task<List<DelmaalSkabelon>> GetAllAsync();
        Task<List<DelmaalSkabelon>> GetByPraktikperiodeNrAsync(int praktikperiodeNr);
        Task<DelmaalSkabelon?> GetByIdAsync(int id);
        Task AddAsync(DelmaalSkabelon model);
        Task UpdateAsync(DelmaalSkabelon model);
        Task DeleteAsync(int id);
    }
}
