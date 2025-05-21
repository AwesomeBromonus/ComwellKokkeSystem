using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IUnderdelmaal
    {
        Task AddAsync(Underdelmaal underdelmaal);
        Task UpdateAsync(Underdelmaal underdelmaal);
        Task<List<Underdelmaal>> GetByDelmaalIdAsync(int delmaalId);
        Task<Underdelmaal?> GetByIdAsync(int id);
        Task UpdateStatusAsync(int underdelmaalId, string nyStatus);
        Task DeleteAsync(int id);
    }

}
