using Modeller;

namespace ComwellKokkeSystem.Service
{
    public interface IUserService
    {
        Task<UserModel?> GetByIdAsync(int id);
        Task<List<UserModel>> GetAllAsync();
        Task<UserModel?> GetByUsernameAsync(string username);
        Task<bool> ValidateLoginAsync(string username, string password);
        Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId);
        Task DeleteAsync(int id);
        Task AssignElevplanToUserAsync(int userId, int elevplanId);
        Task<List<UserModel>> GetAdminsOgKokkeAsync();
        Task UpdateUserAsync(UserModel bruger);

    }
}
