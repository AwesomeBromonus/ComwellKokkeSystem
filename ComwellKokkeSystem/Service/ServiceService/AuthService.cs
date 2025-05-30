using Modeller;
using System.Net.Http.Json;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly IUserStateService _userState;

    public AuthService(HttpClient http, IUserStateService userState)
    {
        _http = http;
        _userState = userState;
    }

    public async Task<bool> Login(UserModel login)
    {
        var response = await _http.PostAsJsonAsync("api/users/login", login);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<UserModel>();
            if (result != null)
            {
                await _userState.SetUserAsync(result);
                return true;
            }
        }

        return false;
    }

    public async Task<UserModel?> GetUserByUsername(string username)
    {
        return await _http.GetFromJsonAsync<UserModel>($"api/users/{username}");
    }

    public async Task<bool> Register(UserModel user)
    { 
        {
            var response = await _http.PostAsJsonAsync("api/users/register", user);
            return response.IsSuccessStatusCode;
        }
    }

    public Task<int?> GetCurrentUserIdAsync()
        => Task.FromResult(_userState.CurrentUser?.Id);

    public Task<string?> GetCurrentUserRoleAsync()
        => Task.FromResult(_userState.CurrentUser?.Role);

    public async Task Logout()
    {
        await _userState.LogoutAsync();
    }

    public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
    {
        return await _http.GetFromJsonAsync<List<UserModel>>("api/users/admins-og-kokke");
    }
}
