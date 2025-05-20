using Modeller;
using System.Threading.Tasks;
namespace ComwellKokkeSystem.Service;

public interface IUserService
{
    Task<UserModel?> GetByIdAsync(int userId);
}