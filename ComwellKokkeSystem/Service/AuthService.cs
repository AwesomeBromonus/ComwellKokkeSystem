using Modeller;
using System.Net.Http.Json;

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
                await _userState.SetUserAsync(result.Username, result.Role, result.Id);
                return true;
            }
        }

        return false;
    }

    public async Task<bool> Register(UserModel user)
    {
        var response = await _http.PostAsJsonAsync("api/users/register", user);
        return response.IsSuccessStatusCode;
    }

    public async Task Logout()
    {
        await _userState.LogoutAsync();
    }

    public Task<int?> GetCurrentUserIdAsync() => Task.FromResult(_userState.Id);
    public Task<string?> GetCurrentUserRoleAsync() => Task.FromResult(_userState.Role);
}
