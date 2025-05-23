using Modeller;

public interface IDelmaalSkabelonService
{
    Task<List<DelmaalSkabelon>> GetAllAsync();
    Task<DelmaalSkabelon?> GetByIdAsync(int id);
    Task AddAsync(DelmaalSkabelon skabelon);
    Task UpdateAsync(DelmaalSkabelon skabelon);
    Task DeleteAsync(int id);
}
