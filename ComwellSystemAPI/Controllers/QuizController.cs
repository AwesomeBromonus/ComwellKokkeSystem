using ComwellSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ComwellSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuiz _quizRepo;
    private readonly IQuestion _questionRepo;
    private readonly IUserRepository _userRepo;

    // Konstruktor med dependency injection af repository til quiz, spørgsmål og brugere
    public QuizController(IQuiz quizRepo, IQuestion questionRepo, IUserRepository userRepo)
    {
        _quizRepo = quizRepo;
        _questionRepo = questionRepo;
        _userRepo = userRepo;
    }

    // GET: api/quiz
    // Henter alle quizzer fra databasen
    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes()
    {
        var quizzes = await _quizRepo.GetQuizzesAsync();
        return Ok(quizzes);
    }

    // GET: api/quiz/{id}
    // Henter en specifik quiz med tilhørende spørgsmål
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQuiz(int id)
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        // Henter alle spørgsmål til den givne quiz ud fra deres id'er
        var questions = new List<Question>();
        foreach (var questionId in quiz.QuestionsIds)
        {
            var question = await _questionRepo.GetQuestionByIdAsync(questionId);
            if (question != null)
            {
                questions.Add(question);
            }
        }

        // Pakker quiz og spørgsmål ind i et samlet objekt til klienten
        var quizWithQuestions = new QuizWithQuestions
        {
            Quiz = quiz,
            Questions = questions
        };

        return Ok(quizWithQuestions);
    }

    // POST: api/quiz
    // Opretter en ny quiz med tilhørende spørgsmål
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        // Validerer at request er korrekt formateret
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request == null || request.Quiz == null || request.Questions == null)
        {
            return BadRequest("Quiz og spørgsmålsdata er påkrævet.");
        }

        // Midlertidigt sættes CreatorUserId til "0" – senere bør autorisation håndteres korrekt
        request.Quiz.CreatorUserId = "0";

        request.Quiz.QuestionsIds = new List<int>();

        // Opretter alle spørgsmål først og gemmer deres id'er i quiz-objektet
        foreach (var question in request.Questions)
        {
            if (string.IsNullOrWhiteSpace(question.Text) || !question.Options.Any())
            {
                return BadRequest("Spørgsmålstekst og mindst én mulighed er påkrævet for hvert spørgsmål.");
            }

            await _questionRepo.CreateQuestionAsync(question);
            request.Quiz.QuestionsIds.Add(question.Id);
        }

        // Opretter selve quizzen efter spørgsmålene er oprettet
        await _quizRepo.CreateQuizAsync(request.Quiz);

        // Returnerer HTTP 201 Created med lokation til den nye quiz
        return CreatedAtAction(nameof(GetQuiz), new { id = request.Quiz.Id }, request.Quiz);
    }

    // PUT: api/quiz/{id}
    // Opdaterer en eksisterende quiz
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateQuiz(int id, [FromBody] Quizzes quiz)
    {
        // Tjek at id i URL matcher id i body
        if (id != quiz.Id)
        {
            return BadRequest("Quiz ID stemmer ikke overens.");
        }

        var existingQuiz = await _quizRepo.GetQuizByIdAsync(id);
        if (existingQuiz == null)
        {
            return NotFound();
        }

        // Her hentes bruger-id fra headeren og valideres
        if (!Request.Headers.TryGetValue("User-Id", out var userIdString) || string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Bruger-ID mangler i anmodningen.");
        }

        if (!int.TryParse(userIdString, out var userId))
        {
            return BadRequest("Ugyldigt bruger-ID format.");
        }

        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized("Brugeren findes ikke.");
        }

        // Kun skaberen af quizzen kan opdatere den
        if (existingQuiz.CreatorUserId != userIdString)
        {
            return Forbid("Du har ikke tilladelse til at opdatere denne quiz.");
        }

        // Opdaterer quiz detaljer og spørgsmål
        existingQuiz.Title = quiz.Title;
        existingQuiz.QuestionsIds = quiz.QuestionsIds;

        await _quizRepo.UpdateQuizAsync(existingQuiz);

        // Returnerer status 204 No Content ved succes
        return NoContent();
    }

    // DELETE: api/quiz/{id}
    // Sletter en quiz og alle tilhørende spørgsmål
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        // Midlertidigt uden autorisation - alle kan slette
        foreach (var questionId in quiz.QuestionsIds)
        {
            await _questionRepo.DeleteQuestionAsync(questionId);
        }

        await _quizRepo.DeleteQuizAsync(id);

        // Returnerer status 204 No Content ved succesfuld sletning
        return NoContent();
    }
}
