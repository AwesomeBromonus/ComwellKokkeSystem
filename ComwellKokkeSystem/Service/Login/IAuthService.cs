using Modeller;

public interface IAuthService
{
    Task<bool> Login(LoginModel login);
    Task<UserModel?> GetUserByUsername(string username);
    Task<bool> Register(UserModel user);
    Task Logout();
    Task<int?> GetCurrentUserIdAsync();
    Task<string?> GetCurrentUserRoleAsync();
}
