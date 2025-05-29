using Modeller;

public interface IDelmaalSkabelonService
{
    Task<List<DelmaalSkabelon>> GetAllAsync();
    Task<DelmaalSkabelon?> GetByIdAsync(int id);
    Task<List<DelmaalSkabelon>> GetByPeriodeAsync(int nummer); // ← denne er ny
    Task AddAsync(DelmaalSkabelon skabelon);
    Task UpdateAsync(DelmaalSkabelon skabelon);
    Task DeleteAsync(int id);
}
