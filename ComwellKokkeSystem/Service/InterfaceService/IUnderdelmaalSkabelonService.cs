using Modeller;

public interface IUnderdelmaalSkabelonService
{
    Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId);
    Task AddAsync(UnderdelmaalSkabelon underdelmaal);
    Task UpdateAsync(UnderdelmaalSkabelon underdelmaal);
    Task DeleteAsync(int id);
}
