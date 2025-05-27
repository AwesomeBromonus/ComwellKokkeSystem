using Microsoft.AspNetCore.Mvc; // Nødvendig for [ApiController], [Route], ControllerBase, IActionResult.
using Modeller; // Refererer til dine fælles datamodeller (Quizzes, Question, CreateQuizRequest, QuizWithQuestions).
using ComwellSystemAPI.Interfaces; // Refererer til dine repository interfaces (IQuiz, IQuestion, IUserRepository).
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Nødvendig for .Any() i ModelState-validering.

namespace ComwellSystemAPI.Controllers;
// Namespace: "Controllers" indeholder dine API-controllers, der eksponerer endpoints for klienter.

[ApiController] // Angiver, at denne klasse er en ASP.NET Core API-controller.
[Route("api/[controller]")] // Definerer basis-URL-routen for denne controller (f.eks. /api/Quiz).
public class QuizController : ControllerBase // Arver fra ControllerBase, der giver adgang til HTTP-specifikke funktioner.
{
    // Dependency Injection: Private readonly felter til at holde instanser af repository-interfaces.
    // Disse injiceres automatisk i konstruktøren via ASP.NET Core's DI-system.
    private readonly IQuiz _quizRepo;
    private readonly IQuestion _questionRepo;
    private readonly IUserRepository _userRepo; // Inkluderet for brugerautorisation.

    /// <summary>
    /// Konstruktør for QuizController. Afhængigheder (repositories) injiceres her.
    /// </summary>
    /// <param name="quizRepo">Implementering af IQuiz til dataadgang for quizzes.</param>
    /// <param name="questionRepo">Implementering af IQuestion til dataadgang for spørgsmål.</param>
    /// <param name="userRepo">Implementering af IUserRepository til brugerdataadgang.</param>
    /// <remarks>
    /// SYSTEMDESIGN: Denne controller følger et tyndt controller-tykt service-mønster (hvis du havde en separat service her).
    /// I din nuværende opsætning, udfører controlleren selv forretningslogik (f.eks. oprettelse af spørgsmål og linkning til quiz).
    /// En alternativ, og ofte bedre, tilgang er at have en 'QuizService' (som du havde før), der håndterer denne logik,
    /// og som derefter injiceres her, så controlleren kun fokuserer på HTTP-specifikke opgaver.
    /// </remarks>
    public QuizController(IQuiz quizRepo, IQuestion questionRepo, IUserRepository userRepo)
    {
        _quizRepo = quizRepo;
        _questionRepo = questionRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// HTTP GET-endpoint for at hente en liste af alle quizzes.
    /// Rute: GET /api/Quiz
    /// </summary>
    /// <returns>HTTP 200 OK med en liste af <see cref="Quizzes"/>-objekter.</returns>
    /// <remarks>
    /// FEJLSØGNINGSTIP: Hvis denne kaldes fra Blazor og returnerer 404 (Not Found) eller 500 (Internal Server Error),
    /// 1. Tjek API'ens URL i Blazor (BaseAddress og endpoint-sti).
    /// 2. Sørg for, at API-projektet kører.
    /// 3. Tjek API'ens serverlogs for exceptions (f.eks. databaseforbindelsesproblemer).
    /// 4. Brug Postman/Swagger til at teste endpointet direkte for at isolere problemet til enten frontend eller backend.
    /// </remarks>
    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes()
    {
        var quizzes = await _quizRepo.GetQuizzesAsync();
        return Ok(quizzes); // Returnerer HTTP 200 OK med quiz-data.
    }

    /// <summary>
    /// HTTP GET-endpoint for at hente en specifik quiz med alle dens tilknyttede spørgsmål.
    /// Rute: GET /api/Quiz/{id}
    /// </summary>
    /// <param name="id">Det unikke integer ID for quizzen.</param>
    /// <returns>HTTP 200 OK med et <see cref="QuizWithQuestions"/>-objekt, eller HTTP 404 Not Found, hvis quizzen ikke eksisterer.</returns>
    /// <remarks>
    /// SYSTEMDESIGN: Denne metode udfører "join"-operationen i applikationslaget. MongoDB er en NoSQL-database,
    /// og den gemmer ikke relationer på samme måde som relationelle databaser. Her opnår du relationen ved:
    /// 1. At gemme spørgsmåls-ID'er (`QuestionsIds`) i `Quizzes`-dokumentet.
    /// 2. Manuelt at hente hvert spørgsmål separat (<see cref="_questionRepo.GetQuestionByIdAsync"/>)
    ///    baseret på de gemte ID'er, efter at quizzen er hentet.
    /// </remarks>
    [HttpGet("{id:int}")] // {id:int} angiver, at 'id' skal være en integer i URL'en.
    public async Task<IActionResult> GetQuiz(int id)
    {
        // Henter quiz-data fra databasen.
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            // FEJLSØGNINGSTIP: Hvis du forventer en quiz, men får 404, tjek om ID'et er korrekt
            // og om quizzen rent faktisk eksisterer i din MongoDB "Quizzes" kollektion.
            return NotFound(); // Returnerer HTTP 404 Not Found.
        }

        // Henter alle spørgsmål til quizzen.
        var questions = new List<Question>();
        // Itererer over hvert spørgsmåls-ID, der er gemt i quizzen.
        foreach (var questionId in quiz.QuestionsIds)
        {
            // Henter hvert enkelt spørgsmål. Dette kan være ineffektivt for quizzer med mange spørgsmål
            // ("N+1 problem"). For optimering, overvej at bruge MongoDBs `$lookup` aggregation pipeline
            // på serversiden, hvis ydeevne bliver et problem.
            var question = await _questionRepo.GetQuestionByIdAsync(questionId);
            if (question != null)
            {
                questions.Add(question);
            }
        }

        // Samler quiz- og spørgsmålsdata i et QuizWithQuestions DTO.
        var quizWithQuestions = new QuizWithQuestions
        {
            Quiz = quiz,
            Questions = questions
        };

        return Ok(quizWithQuestions); // Returnerer HTTP 200 OK med den komplette quiz-struktur.
    }

    /// <summary>
    /// HTTP POST-endpoint for at oprette en ny quiz med dens tilknyttede spørgsmål.
    /// Rute: POST /api/Quiz
    /// </summary>
    /// <param name="request">Et <see cref="CreateQuizRequest"/>-objekt, der indeholder quiz-data og en liste af spørgsmål.</param>
    /// <returns>HTTP 201 Created med placeringen af den nye quiz, eller HTTP 400 Bad Request ved fejl.</returns>
    /// <remarks>
    /// SIKKERHED: Autorisation (f.eks. hvem der må oprette quizzer) er kommenteret ud.
    /// I en ægte applikation ville du validere brugerens identitet her.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        // ModelState.IsValid tjekker DataAnnotations-valideringer på 'request' (f.eks. [Required]).
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returnerer 400 Bad Request med valideringsfejl.
        }

        // Simpel null-validering af indkommende data.
        if (request == null || request.Quiz == null || request.Questions == null)
        {
            return BadRequest("Quiz og spørgsmålsdata er påkrævet.");
        }

        // MIDLERTIDIGT FJERNET AUTORISATION: Sætter en standard CreatorUserId.
        // I en rigtig app ville du hente bruger-ID'et fra autentifikationssystemet (f.eks. ClaimsPrincipal).
        request.Quiz.CreatorUserId = "0"; // Midlertidig standardværdi, bør være det faktiske bruger-ID.
        // UØDVENDIG KODE? Du sætter også CreatorName til string.Empty i Modeller/Quizzes.cs.
        // Hvis du vil have et navn her, skal du tildele det (f.eks. fra brugerens data).
        request.Quiz.CreatorName = "Ukendt Skaber"; // Eksempel på at sætte et navn

        // Rydder QuestionsIds-listen på quiz-objektet for at sikre, at kun de nye ID'er tilføjes.
        request.Quiz.QuestionsIds = new List<int>();

        // Itererer over hvert spørgsmål i anmodningen.
        foreach (var question in request.Questions)
        {
            // Validerer, at spørgsmålsteksten ikke er tom, og at der er mindst ét svaralternativ.
            if (string.IsNullOrWhiteSpace(question.Text) || !question.Options.Any())
            {
                return BadRequest("Spørgsmålstekst og mindst én mulighed er påkrævet for hvert spørgsmål.");
            }

            // Opretter spørgsmålet i databasen. QuestionRepository vil generere dets ID.
            await _questionRepo.CreateQuestionAsync(question);
            // Tilføjer det genererede spørgsmåls-ID til quizzen.
            request.Quiz.QuestionsIds.Add(question.Id);
        }

        // Opretter quizzen i databasen. QuizRepository vil generere dens ID.
        await _quizRepo.CreateQuizAsync(request.Quiz);

        // Returnerer HTTP 201 Created. Dette informerer klienten om, at ressourcen er oprettet,
        // og inkluderer en 'Location' header med URL'en til den nye ressource.
        return CreatedAtAction(nameof(GetQuiz), new { id = request.Quiz.Id }, request.Quiz);
    }

    /// <summary>
    /// HTTP PUT-endpoint for at opdatere en eksisterende quiz.
    /// Rute: PUT /api/Quiz/{id}
    /// </summary>
    /// <param name="id">ID'et for den quiz, der skal opdateres (fra URL'en).</param>
    /// <param name="quiz">Det <see cref="Quizzes"/>-objekt med opdaterede data (fra anmodningsbody).</param>
    /// <returns>HTTP 204 No Content for succes, eller HTTP 400 Bad Request, 404 Not Found, 401 Unauthorized, 403 Forbidden ved fejl.</returns>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateQuiz(int id, [FromBody] Quizzes quiz)
    {
        // Validerer, at ID'et i URL'en stemmer overens med ID'et i anmodningsbody'en.
        if (id != quiz.Id)
        {
            return BadRequest("Quiz ID stemmer ikke overens.");
        }

        // Henter den eksisterende quiz for at verificere dens eksistens og autorisationsformål.
        var existingQuiz = await _quizRepo.GetQuizByIdAsync(id);
        if (existingQuiz == null)
        {
            return NotFound(); // Returnerer 404, hvis quizzen ikke findes.
        }

        // SIKKERHED: Henter bruger-ID fra HTTP-headeren for at validere autorisation.
        if (!Request.Headers.TryGetValue("User-Id", out var userIdString) || string.IsNullOrEmpty(userIdString))
        {
            // FEJLSØGNINGSTIP: Hvis du får 401 her, tjek om din Blazor-klient sender 'User-Id' headeren korrekt.
            return Unauthorized("Bruger-ID mangler i anmodningen.");
        }

        // Validerer, at bruger-ID'et er et gyldigt integer.
        if (!int.TryParse(userIdString, out var userId))
        {
            return BadRequest("Ugyldigt bruger-ID format.");
        }

        // Validerer, at brugeren eksisterer i databasen.
        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized("Brugeren findes ikke.");
        }

        // AUTORISATION: Kontrollerer om den logget ind bruger er den samme som skaberen af quizzen.
        // Bemærk: existingQuiz.CreatorUserId er en string, og userIdString er også en string, så sammenligningen er korrekt.
        if (existingQuiz.CreatorUserId != userIdString)
        {
            // SIKKERHED: En bruger forsøger at opdatere en quiz, de ikke har tilladelse til.
            return Forbid("Du har ikke tilladelse til at opdatere denne quiz."); // Returnerer 403 Forbidden.
        }

        // Opdaterer kun specifikke felter, der må ændres.
        // Det er god praksis kun at opdatere de felter, der sendes fra klienten eller er tilladt at ændre.
        // Her opdateres Title og QuestionsIds.
        existingQuiz.Title = quiz.Title;
        existingQuiz.QuestionsIds = quiz.QuestionsIds; // Vær forsigtig med denne; den tillader klienten at ændre tilknyttede spørgsmål frit.
                                                      // Overvej separate endpoints/logik for at tilføje/fjerne spørgsmål.
        await _quizRepo.UpdateQuizAsync(existingQuiz); // Gemmer de opdaterede data i databasen.
        return NoContent(); // Returnerer 204 No Content for succesfuld opdatering uden body.
    }

    /// <summary>
    /// HTTP DELETE-endpoint for at slette en quiz og alle dens tilknyttede spørgsmål.
    /// Rute: DELETE /api/Quiz/{id}
    /// </summary>
    /// <param name="id">ID'et for den quiz, der skal slettes.</param>
    /// <returns>HTTP 204 No Content for succes, eller HTTP 404 Not Found, hvis quizzen ikke eksisterer.</returns>
    /// <remarks>
    /// SIKKERHED: Autorisation er midlertidigt fjernet. I en ægte applikation skal du validere,
    /// om den logget ind bruger har tilladelse til at slette denne quiz (f.eks. er skaberen eller en administrator).
    /// </remarks>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound(); // Returnerer 404, hvis quizzen ikke findes.
        }

        // MIDLERTIDIGT FJERNET AUTORISATION: Alle kan slette.
        // Før sletning af quizzen slettes alle tilknyttede spørgsmål fra databasen.
        // Dette forhindrer "forældreløse" spørgsmål, når en quiz slettes.
        foreach (var questionId in quiz.QuestionsIds)
        {
            await _questionRepo.DeleteQuestionAsync(questionId);
        }

        // Sletter selve quizzen fra databasen.
        await _quizRepo.DeleteQuizAsync(id);
        return NoContent(); // Returnerer 204 No Content for succesfuld sletning uden body.
    }
}