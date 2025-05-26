using Microsoft.AspNetCore.Mvc;
using Modeller; // Indeholder Quizzes, Question, QuizWithQuestions, CreateQuizRequest
using ComwellSystemAPI.Interfaces; // For IQuiz, IQuestion, IUserRepository
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; // Nødvendig for at få brugerens information

namespace ComwellSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly IQuiz _quizRepo;
    private readonly IQuestion _questionRepo;
    private readonly IUserRepository _userRepo;

    public QuizController(IQuiz quizRepo, IQuestion questionRepo, IUserRepository userRepo)
    {
        _quizRepo = quizRepo;
        _questionRepo = questionRepo;
        _userRepo = userRepo; 
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes()
    {
        var quizzes = await _quizRepo.GetQuizzesAsync();
        return Ok(quizzes);
    }

    // RETTET: id parameter er nu int. Rute constraint er :int
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQuiz(int id) // <--- RETTET til int
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        var questions = new List<Question>();
        // RETTET: questionId er nu int
        foreach (var questionId in quiz.QuestionsIds) // quiz.QuestionsIds er nu List<int>
        {
            var question = await _questionRepo.GetQuestionByIdAsync(questionId); // Brug int questionId
            if (question != null)
            {
                questions.Add(question);
            }
        }

        var quizWithQuestions = new QuizWithQuestions
        {
            Quiz = quiz,
            Questions = questions
        };

        return Ok(quizWithQuestions);
    }

   
    [HttpPost]

    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request == null || request.Quiz == null || request.Questions == null)
        {
            return BadRequest("Quiz og spørgsmålsdata er påkrævet.");
        }
        
        
        // --- Håndtering af QuestionIds i Quiz ---
        request.Quiz.QuestionsIds = new List<int>();

        // --- Opret spørgsmål og tilføj deres ID'er til quizzen ---
        foreach (var question in request.Questions)
        {
            if (string.IsNullOrWhiteSpace(question.Text) || !question.Options.Any())
            {
                return BadRequest("Spørgsmålstekst og mindst én mulighed er påkrævet for hvert spørgsmål.");
            }

            await _questionRepo.CreateQuestionAsync(question);
            request.Quiz.QuestionsIds.Add(question.Id);
        }

        // --- Opret quizzen ---
        await _quizRepo.CreateQuizAsync(request.Quiz);

        return CreatedAtAction(nameof(GetQuiz), new { id = request.Quiz.Id }, request.Quiz);
    }

    // HTTP PUT for at opdatere en eksisterende quiz
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateQuiz(int id, [FromBody] Quizzes quiz)
    {
        if (id != quiz.Id)
        {
            return BadRequest("Quiz ID stemmer ikke overens.");
        }

        var existingQuiz = await _quizRepo.GetQuizByIdAsync(id);
        if (existingQuiz == null)
        {
            return NotFound();
        }

        // --- AUTORISATIONSLOGIK START ---
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserIdString))
        {
            return Unauthorized("Brugeren er ikke logget ind.");
        }

        // Kontroller om den loggede bruger er skaberen af quizzen
        if (existingQuiz.CreatorUserId != currentUserIdString)
        {
            // Hvis brugeren ikke er skaberen, returneres Forbid (403 Forbidden)
            return Forbid("Du har ikke tilladelse til at opdatere denne quiz.");
        }
        // --- AUTORISATIONSLOGIK SLUT ---

        // Kun opdater felter der kan ændres af brugeren, ikke CreatorUserId/Name eller CreatedDate
        existingQuiz.Title = quiz.Title;
        existingQuiz.QuestionsIds = quiz.QuestionsIds; // Hvis du vil tillade at opdatere spørgsmål direkte via quizzen

        await _quizRepo.UpdateQuizAsync(existingQuiz); // Opdater den eksisterende quiz-objekt
        return NoContent();
    }

     [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound(); // Quizzen findes ikke
        }

        // --- AUTORISATIONSLOGIK START ---
        string? currentUserIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserIdString))
        {
            return Unauthorized("Brugeren er ikke logget ind.");
        }

        // Kontroller om den loggede bruger er skaberen af quizzen
        if (quiz.CreatorUserId != currentUserIdString)
        {
            // Hvis brugeren ikke er skaberen, returneres Forbid (403 Forbidden)
            return Forbid("Du har ikke tilladelse til at slette denne quiz.");
        }
        // --- AUTORISATIONSLOGIK SLUT ---

        // Slet først de tilknyttede spørgsmål
        foreach (var questionId in quiz.QuestionsIds)
        {
            await _questionRepo.DeleteQuestionAsync(questionId);
        }
        // Slet derefter selve quizzen
        await _quizRepo.DeleteQuizAsync(id);
        
        return NoContent();
    }
}