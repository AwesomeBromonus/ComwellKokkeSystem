using System.Threading.Tasks;
using Modeller;

public interface IAuthService
{
    Task<bool> Login(LoginModel login);
    Task<bool> Register(UserModel user); // ← ÆNDRET her
    Task Logout();
    Task<int?> GetCurrentUserIdAsync();
    Task<string?> GetCurrentUserRoleAsync();
}
