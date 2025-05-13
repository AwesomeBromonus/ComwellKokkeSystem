using Modeller;

namespace ComwellSystemAPI.Interfaces
{
    public interface IElevplan
    {
        Task<List<Elevplan>> GetAllAsync();
        Task<Elevplan?> GetByIdAsync(int id);
        Task<List<Elevplan>> GetByElevIdAsync(int elevId);
        Task AddAsync(Elevplan elevplan);
        Task UpdateAsync(Elevplan elevplan);
        Task DeleteAsync(int id);
    }
}
