using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service;

public class UserService : IUserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<UserModel?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<UserModel>($"api/users/byid/{id}");
    }
    
}