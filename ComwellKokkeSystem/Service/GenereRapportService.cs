using ComwellKokkeSystem.Service;
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
            return await _httpClient.GetFromJsonAsync<List<Praktikperiode>>($"api/rapport/praktikperioder/{year}");
        }

        public async Task<List<Delmål>?> GetDelmålAsync(int year)
        {
            return await _httpClient.GetFromJsonAsync<List<Modeller.Delmål>>($"api/rapport/delmaal/{year}");
        }

        public async Task<List<UserModel>?> GetBrugereAsync(int year)
        {
            return await _httpClient.GetFromJsonAsync<List<UserModel>>($"api/rapport/brugere/{year}");
        }

        public async Task<byte[]> ExportToCsvAsync(int year)
        {
            var response = await _httpClient.GetAsync($"api/rapport/export/csv/{year}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
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