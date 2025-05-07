using System.Net.Http.Json;
using Modeller;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> Login(LoginModel login)
    {
        var response = await _http.PostAsJsonAsync("api/users/login", login);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Register(RegisterModel user)
    {
        var response = await _http.PostAsJsonAsync("api/users/register", user);
        return response.IsSuccessStatusCode;
    }

    public Task Logout()
    {
        // Kan udvides senere til fx slette localStorage
        return Task.CompletedTask;
    }
}
