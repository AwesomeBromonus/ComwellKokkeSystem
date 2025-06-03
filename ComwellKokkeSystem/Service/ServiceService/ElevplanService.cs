using Modeller;
using System.Net.Http.Json;

namespace ComwellKokkeSystem.Service.Elev;

public class ElevplanService : IElevplanService
{
    private readonly HttpClient _http;

    //Konstruktor
    public ElevplanService(HttpClient http)
    {
        this._http = http;
    }

    public async Task<List<Elevplan>?> GetElevplanerAsync() =>
        await _http.GetFromJsonAsync<List<Elevplan>>("api/elevplan");

    public async Task<Elevplan?> GetElevplanByIdAsync(int id) =>
        await _http.GetFromJsonAsync<Elevplan>($"api/elevplan/{id}");

   

    public async Task AddElevplanAsync(Elevplan plan) =>
        await _http.PostAsJsonAsync("api/elevplan", plan);

    public async Task UpdateElevplanAsync(Elevplan plan) =>
        await _http.PutAsJsonAsync($"api/elevplan/{plan.Id}", plan);

    public async Task DeleteElevplanAsync(int id) =>
        await _http.DeleteAsync($"api/elevplan/{id}");

    public async Task<Elevplan?> GetElevplanForElevAsync(int elevId)
    {
        return await _http.GetFromJsonAsync<Elevplan>($"api/elevplan/elev/{elevId}");
    }




}


