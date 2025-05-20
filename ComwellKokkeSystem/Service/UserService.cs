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
    public async Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId)
    {
        return await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");
    }

    
}