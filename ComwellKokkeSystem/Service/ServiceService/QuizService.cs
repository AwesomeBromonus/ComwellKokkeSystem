using Modeller;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ComwellKokkeSystem.Service.QuizService
{
    public class QuizService : IQuizService
    {
        private readonly HttpClient _httpClient;
        private readonly UserState _userState;

        public QuizService(HttpClient httpClient, UserState userState)
        {
            _httpClient = httpClient;
            _userState = userState;
        }

        public async Task<List<Quizzes>?> GetAllQuizzesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Quizzes>>("api/Quiz");
        }

        public async Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(int quizId)
        {
            return await _httpClient.GetFromJsonAsync<QuizWithQuestions>($"api/Quiz/{quizId}");
        }

        public async Task<bool> CreateQuizAsync(CreateQuizRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Quiz", request);
            return response.IsSuccessStatusCode;
        }

        public async Task UpdateQuizAsync(int quizId, Quizzes quizDto)
        {
            if (_userState.Id == null)
            {
                throw new UnauthorizedAccessException("Brugeren er ikke logget ind.");
            }

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/Quiz/{quizId}");
            requestMessage.Headers.Add("User-Id", _userState.Id.Value.ToString());
            requestMessage.Content = JsonContent.Create(quizDto);

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            var response = await _httpClient.DeleteAsync($"api/Quiz/{quizId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<Modeller.Quizzes?> GetQuizByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Modeller.Quizzes>($"api/Quiz/{id}");
        }
    }
}