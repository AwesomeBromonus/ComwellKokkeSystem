// ComwellKokkeSystem/Service/QuizService/QuizService.cs
using Modeller;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service.QuizService
{
    public class QuizService : IQuizService
    {
        private readonly HttpClient _httpClient;

        public QuizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Quizzes>?> GetAllQuizzesAsync() // Bruger Quizzes
        {
            return await _httpClient.GetFromJsonAsync<List<Quizzes>>("api/Quiz");
        }

        public async Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(string quizId)
        {
            return await _httpClient.GetFromJsonAsync<QuizWithQuestions>($"api/Quiz/{quizId}");
        }

        public async Task CreateQuizAsync(CreateQuizRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Quiz", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateQuizAsync(string quizId, Quizzes quizDto) // Bruger Quizzes
        {
            // Sender Quizzes til API'ets PUT endpoint
            var response = await _httpClient.PutAsJsonAsync($"api/Quiz/{quizId}", quizDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteQuizAsync(string quizId)
        {
            var response = await _httpClient.DeleteAsync($"api/Quiz/{quizId}");
            response.EnsureSuccessStatusCode();
        }
    }
}