using System.Net.Http.Json;
using Modeller;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly UserState _userState;

    public AuthService(HttpClient http, UserState userState)
    {
        _http = http;
        _userState = userState;
    }

    public async Task<bool> Login(LoginModel login)
    {
        var response = await _http.PostAsJsonAsync("api/users/login", login);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result != null)
            {
                // Gem ID også!
                _userState.SetUser(result.Username, result.Role, result.Id);
                return true;
            }
        }

        return false;
    }

    public async Task<bool> Register(UserModel user)  // 🔁 Ændret til UserModel
    {
        var response = await _http.PostAsJsonAsync("api/users/register", user);
        return response.IsSuccessStatusCode;
    }

    public Task Logout()
    {
        _userState.Logout();
        return Task.CompletedTask;
    }

    // Returnerer brugerens ID
    public Task<int?> GetCurrentUserIdAsync()
    {
        return Task.FromResult(_userState.Id);
    }

    // Returnerer brugerens rolle
    public Task<string?> GetCurrentUserRoleAsync()
    {
        return Task.FromResult(_userState.Role);
    }
}
