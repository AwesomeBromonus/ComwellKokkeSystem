using Modeller;

public interface IUserRepository
{
    Task AddAsync(UserModel user);
    Task<List<UserModel>> GetAllAsync();
    Task<UserModel?> GetByEmailAsync(string email);                 // ✅ Tilføjet
    Task<UserModel?> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<bool> ValidateLogin(string email, string password);        // ✅ Opdateret
    Task UpdateUserAsync(UserModel bruger);
    Task AssignElevplanToUserAsync(int userId, int elevplanId);
    Task<List<UserModel>> GetAdminsOgKokkeAsync();
}
