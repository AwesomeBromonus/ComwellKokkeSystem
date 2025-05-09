using Modeller;

public interface IAuthService
{
    Task<bool> Login(LoginModel login);
    Task<bool> Register(RegisterModel user);
    Task Logout();
}
