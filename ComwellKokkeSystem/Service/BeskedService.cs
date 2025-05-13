using Modeller;
using System.Net.Http.Json;

public class BeskedService : IBeskedService
{
    private readonly HttpClient _httpClient;

    public BeskedService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Hent alle beskeder
    public async Task<List<Besked>> GetBeskederAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Besked>>("api/besked");
    }

    // Hent en specifik besked ud fra ID
    public async Task<Besked?> GetBeskedByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Besked>($"api/besked/{id}");
    }

    // Tilføj ny besked
    public async Task AddBeskedAsync(Besked besked)
    {
        await _httpClient.PostAsJsonAsync("api/besked", besked);
    }

    // Opdater eksisterende besked
    public async Task UpdateBeskedAsync(Besked besked)
    {
        await _httpClient.PutAsJsonAsync($"api/besked/{besked.Id}", besked);
    }

    // Slet besked ud fra ID
    public async Task DeleteBeskedAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/besked/{id}");
    }
    // Henter alle beskeder for en bestemt bruger
    public async Task<List<Besked>> GetByUserIdAsync(int userId)
    {
        return await _httpClient.GetFromJsonAsync<List<Besked>>($"api/besked/user/{userId}");
    }
}
