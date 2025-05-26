using Microsoft.AspNetCore.Mvc;
using Modeller; // Indeholder Quizzes, Question, QuizWithQuestions, CreateQuizRequest
using ComwellSystemAPI.Interfaces; // For IQuiz og IQuestion
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Nødvendig for .FirstOrDefault() og .RemoveAll() hvis brugt

namespace ComwellSystemAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    // INGEN QUIZSERVICE HER. Direkte injektion af repositories.
    private readonly IQuiz _quizRepo;
    private readonly IQuestion _questionRepo;

    // Konstruktør injicerer nu IQuiz og IQuestion
    public QuizController(IQuiz quizRepo, IQuestion questionRepo)
    {
        _quizRepo = quizRepo;
        _questionRepo = questionRepo;
    }

    // HTTP GET for at hente alle quizzes
    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes()
    {
        // Kalder direkte repository
        var quizzes = await _quizRepo.GetQuizzesAsync();
        return Ok(quizzes); // Returnerer 200 OK med listen af quizzes
    }

    // HTTP GET for at hente en enkelt quiz med dens spørgsmål ud fra ID
    // GET /api/Quiz/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuiz(string id)
    {
        // Henter quiz direkte fra repository
        var quiz = await _quizRepo.GetQuizByIdAsync(id);
        if (quiz == null)
        {
            return NotFound(); // Returnerer 404 Not Found, hvis quizzen ikke findes
        }

        // Henter tilknyttede spørgsmål direkte fra repository
        var questions = new List<Question>();
        foreach (var questionId in quiz.QuestionsIds)
        {
            var question = await _questionRepo.GetQuestionByIdAsync(questionId);
            if (question != null)
            {
                questions.Add(question);
            }
        }

        // Samler det i QuizWithQuestions DTO'en (som skal ligge i Modeller)
        var quizWithQuestions = new QuizWithQuestions
        {
            Quiz = quiz,
            Questions = questions
        };

        return Ok(quizWithQuestions); // Returnerer 200 OK med quizzen og dens spørgsmål
    }

    // DTO til at oprette en quiz med dens spørgsmål (SKAL LIGGE I MODELLER)
    // using Modeller; skal sikre at denne er tilgængelig
    // public class CreateQuizRequest { ... } // Denne er nu i Modeller-projektet

    // HTTP POST for at oprette en ny quiz med dens spørgsmål
    // POST /api/Quiz
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequest request)
    {
        if (request == null || request.Quiz == null || request.Questions == null)
        {
            return BadRequest("Quiz og spørgsmålsdata er påkrævet.");
        }

        // Håndterer oprettelse af individuelle spørgsmål først, og linker dem derefter til quizzen.
        // Denne logik er flyttet fra "QuizService" (backend) direkte ind i controlleren,
        // da du ikke har et service-lag.
        if (string.IsNullOrEmpty(request.Quiz._id))
        {
            request.Quiz._id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        }
        // Sikrer, at vi kun linker nye spørgsmål.
        // Håndter null-tjek, hvis QuestionsIds kan være null fra klienten.
        if (request.Quiz.QuestionsIds == null)
        {
            request.Quiz.QuestionsIds = new List<string>();
        }
        else
        {
            request.Quiz.QuestionsIds.Clear();
        }
        

        foreach (var question in request.Questions)
        {
            if (string.IsNullOrEmpty(question._id))
            {
                question._id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            }
            await _questionRepo.CreateQuestionAsync(question); // Opret spørgsmålet i databasen
            request.Quiz.QuestionsIds.Add(question._id); // Tilføj spørgsmålets ID til quizzen
        }

        await _quizRepo.CreateQuizAsync(request.Quiz); // Opret quizzen i databasen

        // Returnerer 201 Created med placeringen af den nye quiz
        return CreatedAtAction(nameof(GetQuiz), new { id = request.Quiz._id }, request.Quiz);
    }

    // HTTP PUT for at opdatere en eksisterende quiz
    // PUT /api/Quiz/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuiz(string id, [FromBody] Quizzes quiz)
    {
        if (id != quiz._id)
        {
            return BadRequest("Quiz ID stemmer ikke overens.");
        }

        // Tjekker eksistens ved at hente fra repository
        var existingQuiz = await _quizRepo.GetQuizByIdAsync(id); // Direkte kald til _quizRepo
        if (existingQuiz == null)
        {
            return NotFound();
        }

        // Opdaterer quiz direkte via repository
        await _quizRepo.UpdateQuizAsync(quiz);
        return NoContent(); // 204 No Content for succesfuld opdatering
    }

    // HTTP DELETE for at slette en quiz og dens tilknyttede spørgsmål
    // DELETE /api/Quiz/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuiz(string id)
    {
        // Henter quiz for at finde tilknyttede spørgsmål
        var quiz = await _quizRepo.GetQuizByIdAsync(id); // Direkte kald til _quizRepo
        if (quiz != null)
        {
            // Slet først de tilknyttede spørgsmål
            foreach (var questionId in quiz.QuestionsIds)
            {
                await _questionRepo.DeleteQuestionAsync(questionId); // Direkte kald til _questionRepo
            }
            // Slet derefter selve quizzen
            await _quizRepo.DeleteQuizAsync(id); // Direkte kald til _quizRepo
        }
        return NoContent(); // 204 No Content for succesfuld sletning
    }
}