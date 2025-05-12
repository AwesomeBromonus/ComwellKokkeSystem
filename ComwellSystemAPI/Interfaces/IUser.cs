using Modeller;

public interface IUserRepository
{
    Task<UserModel?> GetByUsernameAsync(string username);
    Task<UserModel?> GetByIdAsync(int id);
    Task AddAsync(UserModel user);
    Task<bool> ValidateLogin(string username, string password);
    Task<List<UserModel>> GetAllAsync();
    Task DeleteAsync(int id);
}
