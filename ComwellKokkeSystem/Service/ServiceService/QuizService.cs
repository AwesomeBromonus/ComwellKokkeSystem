using Modeller;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service.QuizService
{
    // @* KLASSE: Serviceklasse der implementerer IQuizService til håndtering af quizzer via HTTP-requests *@
    public class QuizService : IQuizService
    {
        private readonly HttpClient _httpClient;
        private readonly IUserStateService _userState;

        public QuizService(HttpClient httpClient, IUserStateService userState)
        {
            _httpClient = httpClient;
            _userState = userState;
        }

        // @* Henter alle quizzer via GET *@
        public async Task<List<Quizzes>?> GetAllQuizzesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Quizzes>>("api/Quiz");
        }

        // @* Henter en quiz med tilhørende spørgsmål via GET *@
        public async Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(int quizId)
        {
            return await _httpClient.GetFromJsonAsync<QuizWithQuestions>($"api/Quiz/{quizId}");
        }

        // @* Opretter en ny quiz via POST *@
        public async Task<bool> CreateQuizAsync(CreateQuizRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Quiz", request);
            return response.IsSuccessStatusCode;
        }

        // @* Opdaterer en eksisterende quiz via PUT, kræver at bruger er logget ind *@
        public async Task UpdateQuizAsync(int quizId, Quizzes quizDto)
        {
            if (_userState.Id == null)
                throw new UnauthorizedAccessException("Brugeren er ikke logget ind.");

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/Quiz/{quizId}");
            requestMessage.Headers.Add("User-Id", _userState.Id.Value.ToString());
            requestMessage.Content = JsonContent.Create(quizDto);

            var response = await _httpClient.SendAsync(requestMessage);
            response.EnsureSuccessStatusCode();
        }

        // @* Sletter en quiz via DELETE efter id *@
        public async Task DeleteQuizAsync(int quizId)
        {
            var response = await _httpClient.DeleteAsync($"api/Quiz/{quizId}");
            response.EnsureSuccessStatusCode();
        }

        // @* Henter en quiz efter id via GET *@
        public async Task<Quizzes?> GetQuizByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Quizzes>($"api/Quiz/{id}");
        }
    }
}
