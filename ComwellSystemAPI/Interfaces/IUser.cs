using Modeller;

public interface IUserRepository
{
    Task AddAsync(UserModel user);
    Task<List<UserModel>> GetAllAsync();
    Task<UserModel?> GetByUsernameAsync(string username);
    Task<UserModel?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<bool> ValidateLogin(string username, string password);
    Task UpdateUserAsync(UserModel bruger);
    Task AssignElevplanToUserAsync(int userId, int elevplanId);
    Task<List<UserModel>> GetAdminsOgKokkeAsync();

    

}
