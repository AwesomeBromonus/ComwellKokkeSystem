using ComwellSystemAPI.Interfaces; // Refererer til din IQuiz interface.
using Modeller; // Refererer til dine fælles datamodeller.
using MongoDB.Driver; // Nødvendig for at interagere med MongoDB.
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Nødvendig for LINQ-operationer (ikke direkte brugt i din kode her, men ofte nyttig).
using System; // Nødvendig for Exception og Console.WriteLine.

namespace ComwellSystemAPI.Repositories;
// Namespace: "Repositories" indeholder implementeringer for dataadgangslag.

/// <summary>
/// Implementerer <see cref="IQuiz"/> interfacet og håndterer alle databaseoperationer
/// relateret til <see cref="Quizzes"/>-objekter i MongoDB.
/// </summary>
public class QuizRepositoryMongoDB : IQuiz
{
    // En privat felt, der repræsenterer MongoDB-kollektionen for quizzes.
    private readonly IMongoCollection<Quizzes> _quizzes;

    /// <summary>
    /// Konstruktør for QuizRepositoryMongoDB. Etablerer forbindelse til MongoDB.
    /// </summary>
    /// <remarks>
    /// UØDVENDIG KODE? Som i <see cref="QuestionRepository"/> er direkte instansiering af MongoClient her
    /// ikke den bedste praksis for Dependency Injection (DI) i ASP.NET Core.
    /// Flyt forbindelsesopsætningen til `Program.cs` og injicer `IMongoDatabase` her.
    /// Dette gør koden mere testbar og robust.
    /// </remarks>
    public QuizRepositoryMongoDB()
    {
        try
        {
            // Opretter en ny MongoDB-klient. ADVARSEL: Hardkodet forbindelsesstreng. Flyt til appsettings.json.
            var client = new MongoClient("mongodb+srv://Bromus:Bromus12344321@cluster0.k4kon.mongodb.net/");
            // Henter "Comwell" databasen.
            var db = client.GetDatabase("Comwell");
            // Henter "Quizzes" kollektionen. MongoDB opretter den automatisk, hvis den ikke eksisterer.
            _quizzes = db.GetCollection<Quizzes>("Quizzes");
            Console.WriteLine("Forbindelse til MongoDB etableret."); // Godt til debugging/logging.
        }
        catch (Exception ex)
        {
            // Fejlsøgningstip: Hvis denne fejl opstår, tjek din forbindelsesstreng, netværksadgang
            // og om MongoDB-serveren er tilgængelig.
            Console.WriteLine($"Fejl ved forbindelse til MongoDB: {ex.Message}");
            throw; // Kaster exception videre, så API'en ved, at der er et problem.
        }
    }

    /// <summary>
    /// Henter alle quizzes fra "Quizzes" kollektionen.
    /// </summary>
    /// <returns>En liste af alle <see cref="Quizzes"/>-objekter.</returns>
    public async Task<List<Quizzes>> GetQuizzesAsync()
    {
        try
        {
            // Finder alle dokumenter i kollektionen og konverterer dem til en liste.
            var quizzes = await _quizzes.Find(_ => true).ToListAsync();
            Console.WriteLine($"Hentede {quizzes.Count} quizzer fra databasen."); // Godt til logging.
            return quizzes;
        }
        catch (Exception ex)
        {
            // Fejlsøgningstip: Hvis hentning mislykkes, kan det skyldes databaseforbindelse,
            // netværksproblemer, eller at kollektionen slet ikke eksisterer (dog oprettes den ved første indsættelse).
            Console.WriteLine($"Fejl ved hentning af quizzer: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Henter en enkelt quiz ud fra dens unikke ID.
    /// </summary>
    /// <param name="id">ID'et for den quiz, der skal hentes.</param>
    /// <returns><see cref="Quizzes"/>-objektet, hvis fundet; ellers null.</returns>
    public async Task<Quizzes> GetQuizByIdAsync(int id)
    {
        try
        {
            // Finder den første quiz, hvis 'Id' matcher det givne 'id'.
            var quiz = await _quizzes.Find(q => q.Id == id).FirstOrDefaultAsync();
            Console.WriteLine(quiz != null ? $"Fundet quiz med ID {id}." : $"Quiz med ID {id} blev ikke fundet.");
            return quiz;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved hentning af quiz med ID {id}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Opretter en ny quiz i "Quizzes" kollektionen.
    /// </summary>
    /// <param name="quiz">Det <see cref="Quizzes"/>-objekt, der skal oprettes.</param>
    public async Task CreateQuizAsync(Quizzes quiz)
    {
        try
        {
            // Genererer et nyt unikt ID til quizzen, før den indsættes.
            // Nødvendigt pga. brugen af 'int' som ID, se bemærkning i QuestionRepository.
            quiz.Id = await GetNextQuizIdAsync();
            await _quizzes.InsertOneAsync(quiz);
            Console.WriteLine($"Quiz med ID {quiz.Id} blev oprettet.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved oprettelse af quiz: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Opdaterer en eksisterende quiz i "Quizzes" kollektionen.
    /// </summary>
    /// <param name="quizDto">Det <see cref="Quizzes"/>-objekt med opdaterede værdier. 'Id' bruges til at finde dokumentet.</param>
    /// <remarks>
    /// Navngivningen af parameteren `quizDto` er lidt misvisende her, da det er selve domæneobjektet `Quizzes`
    /// der modtages, ikke en særskilt DTO. Men det er en lille detalje.
    /// </remarks>
    public async Task UpdateQuizAsync(Quizzes quizDto)
    {
        try
        {
            // Erstatter et eksisterende quiz-dokument baseret på dets ID med det nye 'quizDto' objekt.
            await _quizzes.ReplaceOneAsync(q => q.Id == quizDto.Id, quizDto);
            Console.WriteLine($"Quiz med ID {quizDto.Id} blev opdateret.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved opdatering af quiz med ID {quizDto.Id}: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Genererer det næste sekventielle ID for en ny quiz.
    /// Finder det højeste eksisterende 'Id' i kollektionen og tilføjer 1.
    /// </summary>
    /// <returns>Det næste ledige ID.</returns>
    /// <remarks>
    /// DATABASEDESIGN / SIKKERHEDSADVARSEL:
    /// Som i <see cref="QuestionRepository.GetNextQuestionIdAsync"/> kan denne metode lide af race conditions
    /// i et høj-konkurrence miljø. Overvej atomisk ID-generering eller MongoDB ObjectIds for robusthed.
    /// </remarks>
    private async Task<int> GetNextQuizIdAsync()
    {
        try
        {
            // Sorterer quizzer faldende efter ID for at finde det højeste.
            var sort = Builders<Quizzes>.Sort.Descending(q => q.Id);
            // Henter den quiz med det højeste ID.
            var lastQuiz = await _quizzes.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
            // Bestemmer næste ID.
            int nextId = lastQuiz == null ? 1 : lastQuiz.Id + 1;
            Console.WriteLine($"Næste quiz-ID genereret: {nextId}");
            return nextId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved generering af næste quiz-ID: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Sletter en quiz fra "Quizzes" kollektionen ud fra dens ID.
    /// </summary>
    /// <param name="id">ID'et for den quiz, der skal slettes.</param>
    public async Task DeleteQuizAsync(int id)
    {
        try
        {
            // Sletter det første quiz-dokument, der matcher det givne ID.
            await _quizzes.DeleteOneAsync(q => q.Id == id);
            Console.WriteLine($"Quiz med ID {id} blev slettet.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved sletning af quiz med ID {id}: {ex.Message}");
            throw;
        }
    }
}