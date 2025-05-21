using Modeller;

namespace ComwellSystemAPI.Interfaces
{
  

    public interface IUnderdelmaalSkabelon
    {
        Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId);
        Task AddAsync(UnderdelmaalSkabelon item);
        Task DeleteAsync(int id);
        Task UpdateAsync(UnderdelmaalSkabelon model);

    }
}
