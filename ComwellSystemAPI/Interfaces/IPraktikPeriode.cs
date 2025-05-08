
using Modeller;


namespace ComwellSystemAPI.Interfaces
{
    public interface IPraktikperiode
    {
        Task<List<Praktikperiode>> GetAllAsync();
        Task<Praktikperiode?> GetByIdAsync(int id);
        Task AddAsync(Praktikperiode periode);
        Task UpdateAsync(Praktikperiode periode);
        Task DeleteAsync(int id);

        Task UpdateDelmålAsync(int praktikperiodeId, int delmålId, string nyStatus);

    }

}
