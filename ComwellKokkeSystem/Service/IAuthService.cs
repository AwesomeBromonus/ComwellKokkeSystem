using System.Threading.Tasks;
using Modeller;

public interface IAuthService
{
    // Login med brugernavn og kodeord
    Task<bool> Login(LoginModel login);

    // Registrer ny bruger
    Task<bool> Register(RegisterModel user);

    // Log ud
    Task Logout();

    // Hent ID på den aktuelt loggede bruger
    Task<int?> GetCurrentUserIdAsync();

    // Hent rolle på den aktuelt loggede bruger (fx "admin" eller "elev")
    Task<string?> GetCurrentUserRoleAsync();
}
