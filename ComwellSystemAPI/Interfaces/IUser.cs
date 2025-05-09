using Modeller;

namespace ComwellSystemAPI.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel?> GetByUsernameAsync(string username);
        Task AddAsync(UserModel user);
        Task<bool> ValidateLogin(string username, string password);
    }
}
