using Modeller;

public interface IUnderdelmaalSkabelonService
{
    Task<List<UnderdelmaalSkabelon>> GetByDelmaalSkabelonIdAsync(int delmaalSkabelonId);
    Task AddAsync(UnderdelmaalSkabelon skabelon);
    Task DeleteAsync(int id);
    Task UpdateAsync(UnderdelmaalSkabelon skabelon);

}
