using Microsoft.AspNetCore.Mvc;
using Modeller;
using ComwellSystemAPI.Interfaces;
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetQuiz(int id)
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        var questions = new List<Question>();
        foreach (var questionId in quiz.QuestionsIds)
        {
            var question = await _questionRepo.GetQuestionByIdAsync(questionId);
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

        // MIDLERTIDIGT FJERNET AUTORISATION - SÆTTER STANDARD CREATORUSERID
        request.Quiz.CreatorUserId = "0"; // Midlertidig standardværdi

        request.Quiz.QuestionsIds = new List<int>();
        foreach (var question in request.Questions)
        {
            if (string.IsNullOrWhiteSpace(question.Text) || !question.Options.Any())
            {
                return BadRequest("Spørgsmålstekst og mindst én mulighed er påkrævet for hvert spørgsmål.");
            }

            await _questionRepo.CreateQuestionAsync(question);
            request.Quiz.QuestionsIds.Add(question.Id);
        }

        await _quizRepo.CreateQuizAsync(request.Quiz);
        return CreatedAtAction(nameof(GetQuiz), new { id = request.Quiz.Id }, request.Quiz);
    }

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

        // Hent brugerens ID fra headeren
        if (!Request.Headers.TryGetValue("User-Id", out var userIdString) || string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized("Bruger-ID mangler i anmodningen.");
        }

        // Valider brugeren eksisterer
        if (!int.TryParse(userIdString, out var userId))
        {
            return BadRequest("Ugyldigt bruger-ID format.");
        }

        var user = await _userRepo.GetByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized("Brugeren findes ikke.");
        }

        // Kontroller om brugeren er skaberen af quizzen
        if (existingQuiz.CreatorUserId != userIdString)
        {
            return Forbid("Du har ikke tilladelse til at opdatere denne quiz.");
        }

        existingQuiz.Title = quiz.Title;
        existingQuiz.QuestionsIds = quiz.QuestionsIds;
        await _quizRepo.UpdateQuizAsync(existingQuiz);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        // MIDLERTIDIGT FJERNET AUTORISATION - ALLE KAN SLETTE
        foreach (var questionId in quiz.QuestionsIds)
        {
            await _questionRepo.DeleteQuestionAsync(questionId);
        }

        await _quizRepo.DeleteQuizAsync(id);
        return NoContent();
    }
}