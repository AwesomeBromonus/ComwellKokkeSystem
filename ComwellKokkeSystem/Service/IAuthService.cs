using System.Threading.Tasks;
using Modeller;

public interface IAuthService
{
    Task<bool> Login(LoginModel login);
    Task<bool> Register(UserModel user); // 🔁 Ændret her
    Task Logout();
}
