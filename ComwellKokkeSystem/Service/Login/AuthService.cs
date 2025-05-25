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
                int? hotelId = result.HotelId;
                int? elevplanId = result.ElevplanId;

                await _userState.SetUserAsync(
                    result.Username,
                    result.Role,
                    result.Id,
                    hotelId,
                    elevplanId,
                    result.Navn,
                    result.Email
                );

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
        try
        {
            var response = await _http.PostAsJsonAsync("api/users/register", user);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            Console.WriteLine($"Fejl ved registrering: {user.Username}");
            return false;
        }
    }

    public Task<int?> GetCurrentUserIdAsync() => Task.FromResult(_userState.Id);

    public Task<string?> GetCurrentUserRoleAsync() => Task.FromResult(_userState.Role);

    public async Task Logout()
    {
        await _userState.LogoutAsync();
    }

    public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
    {
        return await _http.GetFromJsonAsync<List<UserModel>>("api/users/admins-og-kokke");
    }
}
