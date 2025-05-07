using Modeller;

public interface IElevplan
{
    Task<List<Elevplan>> GetAllAsync();
    Task<Elevplan?> GetByIdAsync(int id);
    Task AddAsync(Elevplan plan);
    Task UpdateAsync(Elevplan plan);
    Task DeleteAsync(int id);
}
