using Modeller;

public interface IBesked
{
    Task<List<Besked>> GetAllAsync();
    Task<Besked?> GetByIdAsync(int id);
    Task AddAsync(Besked besked);
    Task UpdateAsync(Besked besked);
    Task DeleteAsync(int id);
}