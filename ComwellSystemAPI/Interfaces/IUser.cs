using Modeller;

public interface IUserRepository
{
    Task<Bruger?> GetByUsernameAsync(string username);
    Task<Bruger?> GetByIdAsync(int id);
    Task AddAsync(Bruger bruger);
    Task<bool> ValidateLogin(string username, string password);
    Task<List<Bruger>> GetAllAsync();
    Task DeleteAsync(int id);
}
