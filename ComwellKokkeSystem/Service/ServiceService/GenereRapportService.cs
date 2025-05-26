// ComwellKokkeSystem.Service/GenereRapportService.cs
using Modeller;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service
{
    public class GenereRapportService : IGenereRapportService
    {
        private readonly HttpClient _httpClient;

        public GenereRapportService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Praktikperiode>?> GetPraktikPerioderAsync(int year)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/rapport/praktikperioder/{year}");
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to fetch praktikperioder for year {year}. Status: {response.StatusCode}, Error: {errorContent}");
                    return new List<Praktikperiode>();
                }
                return await response.Content.ReadFromJsonAsync<List<Praktikperiode>>() ?? new List<Praktikperiode>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPraktikPerioderAsync: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return new List<Praktikperiode>();
            }
        }

        // NYT: Denne metode kalder nu det nye/opdaterede API-endpoint
        public async Task<List<Delmål>?> GetDelmålWithUnderdelmaalAsync(int year)
        {
            try
            {
                // Kalder det nye API-endpoint
                return await _httpClient.GetFromJsonAsync<List<Delmål>>($"api/rapport/delmaal-with-underdelmaal/{year}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching delmål with underdelmål for year {year}: {ex.Message}");
                return new List<Delmål>();
            }
        }

        // Duplikerede metoder er fjernet herfra: GetPraktikPerioderAsync og GetDelmålAsync (den gamle version)

        // Den gamle GetDelmålAsync er sandsynligvis ikke længere nødvendig, hvis den ikke henter underdelmål
        // Hvis den bruges andre steder, skal du overveje at omdøbe den eller opdatere de steder.
        // public async Task<List<Delmål>?> GetDelmålAsync(int year)
        // {
        //     return await _httpClient.GetFromJsonAsync<List<Delmål>>($"api/rapport/delmaal/{year}");
        // }

        public async Task<List<UserModel>?> GetBrugereAsync(int year)
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/rapport/brugere/{year}");
        }

        public async Task<byte[]> ExportToExcelAsync(int year)
        {
            var response = await _httpClient.GetAsync($"api/rapport/export/excel/{year}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
        
        public async Task<List<UserModel>> GetEleverAsync(int year)
        {
            try
            {
                var elever = await _httpClient.GetFromJsonAsync<List<UserModel>>
                    ($"api/users/elever/{year}");
                return elever ?? new List<UserModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching elever for year {year}: {ex}");
                return new List<UserModel>();
            }
        }
    }
}