using Modeller;

public interface IAuthService
{
    Task<bool> Login(LoginModel login);
    Task<UserModel?> GetUserByEmail(string email);   // ✅ Email bruges som login-nøgle
    Task<bool> Register(UserModel user);
    Task Logout();
    Task<int?> GetCurrentUserIdAsync();
    Task<string?> GetCurrentUserRoleAsync();
    Task<List<UserModel>> GetAdminsOgKokkeAsync();
}
