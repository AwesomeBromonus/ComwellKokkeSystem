using Modeller;
using System.Net.Http.Json;
using System.Security.Claims;
namespace ComwellKokkeSystem.Service
{
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

        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<UserModel>>("api/users/all");
        }

        public async Task<UserModel?> GetByUsernameAsync(string username)
        {
            return await _http.GetFromJsonAsync<UserModel>($"api/users/{username}");

        }

        public async Task<bool> ValidateLoginAsync(string username, string password)
        {
            var response = await _http.PostAsJsonAsync("api/users/login", new { username, password });
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Praktikperiode>> GetPraktikperioderForElevAsync(int elevId)
        {
            return await _http.GetFromJsonAsync<List<Praktikperiode>>($"api/praktikperiode/elev/{elevId}");
        }

        public async Task DeleteAsync(int id)
        {
            await _http.DeleteAsync($"api/users/{id}");
        }

        public async Task AssignElevplanToUserAsync(int userId, int elevplanId)
        {
            await _http.PutAsJsonAsync($"api/users/{userId}/assign-elevplan", elevplanId);
        }


        public async Task<List<UserModel>> GetAdminsOgKokkeAsync()
        {
            return await _http.GetFromJsonAsync<List<UserModel>>("api/users/admins-og-kokke");
        }
        public async Task UpdateUserAsync(UserModel bruger)
        {
            await _http.PutAsJsonAsync($"api/users/{bruger.Id}", bruger);
        }


    }
}
