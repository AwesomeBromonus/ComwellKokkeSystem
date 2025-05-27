using Modeller; // Refererer til dine fælles datamodeller (Quizzes, Question, CreateQuizRequest, QuizWithQuestions).
using System.Net.Http; // Nødvendig for at foretage HTTP-kald.
using System.Net.Http.Json; // Udvidelsesmetoder for JSON-serialisering/deserialisering (GetFromJsonAsync, PostAsJsonAsync).
using System.Collections.Generic;
using System.Threading.Tasks;
using System; // Nødvendig for UnauthorizedAccessException.

namespace ComwellKokkeSystem.Service.QuizService;
// Namespace: "Service.QuizService" indeholder klient-side services, der kommunikerer med backend API'en.
// Dette er dit API-klientlag i Blazor-appen.

/// <summary>
/// Klient-side service, der fungerer som et mellemlag for at interagere med Quiz-relaterede endpoints på backend API'en.
/// Den bruger <see cref="HttpClient"/> til at sende HTTP-anmodninger.
/// </summary>
/// <remarks>
/// Dependency Injection: HttpClient er injiceret via konstruktøren.
/// Den er konfigureret i Blazor WASM's `Program.cs` med en base-URL til din API.
/// `UserState` injiceres også for at få adgang til den aktuelle bruger-ID.
/// </remarks>
public class QuizService : IQuizService // Implementerer IQuizService (klient-side interface).
{
    private readonly HttpClient _httpClient; // Bruges til at sende HTTP-anmodninger til API'en.
    private readonly UserState _userState;   // Bruges til at hente den logget ind brugers ID for autorisation.

    /// <summary>
    /// Konstruktør for QuizService (klient-side).
    /// </summary>
    /// <param name="httpClient">En instans af HttpClient, injiceret af Blazor's DI-system.</param>
    /// <param name="userState">En instans af UserState, injiceret af Blazor's DI-system, indeholder information om den logget ind bruger.</param>
    public QuizService(HttpClient httpClient, UserState userState)
    {
        _httpClient = httpClient;
        _userState = userState;
    }

    /// <summary>
    /// Henter alle quizzes fra backend API'en.
    /// </summary>
    /// <returns>En liste af <see cref="Quizzes"/>-objekter, eller null hvis der opstår en fejl.</returns>
    /// <remarks>
    /// HTTP-kald: GET /api/Quiz
    /// </remarks>
    public async Task<List<Quizzes>?> GetAllQuizzesAsync()
    {
        // GetFromJsonAsync foretager en GET-anmodning og forsøger at deserialisere JSON-svaret
        // til en List<Quizzes>.
        return await _httpClient.GetFromJsonAsync<List<Quizzes>>("api/Quiz");
    }

    /// <summary>
    /// Henter en specifik quiz med dens tilknyttede spørgsmål fra backend API'en.
    /// </summary>
    /// <param name="quizId">ID'et for den quiz, der skal hentes.</param>
    /// <returns>Et <see cref="QuizWithQuestions"/>-objekt, eller null hvis quizzen ikke findes/fejl opstår.</returns>
    /// <remarks>
    /// HTTP-kald: GET /api/Quiz/{quizId}
    /// </remarks>
    public async Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(int quizId)
    {
        return await _httpClient.GetFromJsonAsync<QuizWithQuestions>($"api/Quiz/{quizId}");
    }

    /// <summary>
    /// Sender en anmodning til backend API'en om at oprette en ny quiz med dens spørgsmål.
    /// </summary>
    /// <param name="request">Et <see cref="CreateQuizRequest"/>-objekt indeholdende quiz- og spørgsmålsdata.</param>
    /// <returns>Et Task, der repræsenterer den asynkrone operation.</returns>
    /// <remarks>
    /// HTTP-kald: POST /api/Quiz
    /// SIKKERHED: Autorisation (via User-Id header) er kommenteret ud i controlleren for oprettelse.
    /// Dette ville typisk blive tilføjet for at identificere skaberen af quizzen.
    /// `EnsureSuccessStatusCode()` kaster en HttpRequestException, hvis svarets statuskode
    /// ikke indikerer succes (f.eks. 400 Bad Request, 500 Internal Server Error).
    /// FEJLSØGNINGSTIP: Brug browserens udviklerværktøjer (Netværk fanebladet) til at inspicere
    /// det POST-kald, der sendes, og det svar, der modtages. Tjek statuskode og response body.
    /// </remarks>
    public async Task CreateQuizAsync(CreateQuizRequest request)
    {
        // PostAsJsonAsync serialiserer 'request' til JSON og sender det som en POST-anmodning.
        var response = await _httpClient.PostAsJsonAsync("api/Quiz", request);
        response.EnsureSuccessStatusCode(); // Kaster exception ved HTTP-fejl.
    }

    /// <summary>
    /// Sender en anmodning til backend API'en om at opdatere en eksisterende quiz.
    /// </summary>
    /// <param name="quizId">ID'et for den quiz, der skal opdateres.</param>
    /// <param name="quizDto">Det <see cref="Quizzes"/>-objekt med opdaterede data.</param>
    /// <returns>Et Task, der repræsenterer den asynkrone operation.</returns>
    /// <remarks>
    /// HTTP-kald: PUT /api/Quiz/{quizId}
    /// SIKKERHED: Denne metode inkluderer brugerens ID i en HTTP-header ('User-Id'),
    /// som API'en bruger til at validere, om brugeren har tilladelse til at opdatere quizzen.
    /// </remarks>
    public async Task UpdateQuizAsync(int quizId, Quizzes quizDto)
    {
        // SIKKERHED: Tjekker om brugeren er logget ind, før anmodningen sendes.
        if (_userState.Id == null)
        {
            // FEJLSØGNINGSTIP: Hvis denne exception kastes uventet, tjek 'UserState' og AuthService.
            throw new UnauthorizedAccessException("Brugeren er ikke logget ind.");
        }

        // Opretter en HttpRequestMessage for at kunne tilføje custom headers.
        var requestMessage = new HttpRequestMessage(HttpMethod.Put, $"api/Quiz/{quizId}");
        // Tilføjer bruger-ID'et til anmodningsheaderen.
        requestMessage.Headers.Add("User-Id", _userState.Id.Value.ToString());
        // Sætter indholdet af anmodningen til JSON-serialisering af quizDto.
        requestMessage.Content = JsonContent.Create(quizDto);

        // Sender anmodningen.
        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode(); // Kaster exception ved HTTP-fejl.
    }

    /// <summary>
    /// Sender en anmodning til backend API'en om at slette en quiz.
    /// </summary>
    /// <param name="quizId">ID'et for den quiz, der skal slettes.</param>
    /// <returns>Et Task, der repræsenterer den asynkrone operation.</returns>
    /// <remarks>
    /// HTTP-kald: DELETE /api/Quiz/{quizId}
    /// SIKKERHED: I controlleren er autorisation for sletning midlertidigt fjernet.
    /// Overvej at tilføje bruger-ID header og verificere rettigheder på serversiden.
    /// </remarks>
    public async Task DeleteQuizAsync(int quizId)
    {
        var response = await _httpClient.DeleteAsync($"api/Quiz/{quizId}");
        response.EnsureSuccessStatusCode(); // Kaster exception ved HTTP-fejl.
    }

    // UØDVENDIG KODE?
    // Du har allerede 'GetQuizWithQuestionsAsync(int quizId)', som henter en quiz med alle dens spørgsmål.
    // Denne 'GetQuizByIdAsync(int id)' er en duplikering af funktionalitet og returnerer kun 'Quizzes'
    // uden de relaterede spørgsmål. Medmindre du har et specifikt UI-behov for kun quiz-metadata,
    // er denne metode sandsynligvis unødvendig og kan fjernes.
    /// <summary>
    /// Henter et enkelt quiz-objekt fra backend API'en baseret på dets ID.
    /// </summary>
    /// <param name="id">ID'et for den quiz, der skal hentes.</param>
    /// <returns>Et <see cref="Quizzes"/>-objekt, eller null.</returns>
    /// <remarks>
    /// HTTP-kald: GET /api/Quiz/{id}
    /// </remarks>
    public async Task<Modeller.Quizzes?> GetQuizByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Modeller.Quizzes>($"api/Quiz/{id}");
    }
}