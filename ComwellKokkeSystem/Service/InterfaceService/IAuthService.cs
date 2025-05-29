using Modeller;

public interface IAuthService
{
    Task<bool> Login(UserModel login);
    Task<UserModel?> GetUserByUsername(string username);
    Task<bool> Register(UserModel user);
    Task Logout();
    Task<int?> GetCurrentUserIdAsync();
    Task<string?> GetCurrentUserRoleAsync();
    Task<List<UserModel>> GetAdminsOgKokkeAsync();
}
