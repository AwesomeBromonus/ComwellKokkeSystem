using ComwellSystemAPI.Interfaces; // Bruger din IQuiz interface
using Modeller;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Repositories;

// Implementering af IQuiz interfacet, der interagerer med MongoDB for Quizzes
public class QuizRepositoryMongoDB : IQuiz // Implementerer IQuiz, som du ønskede
{
    private readonly IMongoCollection<Quizzes> _quizzes; // MongoDB collection til quizzes

    // Konstruktør: IMongoDatabase injiceres her fra Dependency Injection (opsat i API'ets Program.cs)
    public QuizRepositoryMongoDB() // <--- Ingen parameter her
    {
        // Genindsæt manuel oprettelse af klient og database
        var client = new MongoClient("mongodb+srv://Bromus:Bromus12344321@cluster0.k4kon.mongodb.net/"); // Din egen forbindelsesstreng
        var db = client.GetDatabase("Comwell"); // Din egen database
        _quizzes = db.GetCollection<Quizzes>("Quizzes");
    }

    // Henter alle quizzes fra databasen
    public async Task<List<Quizzes>> GetQuizzesAsync() // Navnet matcher IQuiz
    {
        return await _quizzes.Find(quiz => true).ToListAsync();
    }
    
    // Henter en enkelt quiz ud fra dens ID
    public async Task<Quizzes> GetQuizByIdAsync(string id) // ID er string
    {
        return await _quizzes.Find(q => q._id == id).FirstOrDefaultAsync();
    }
    
    // Opretter en ny quiz i databasen
    public async Task CreateQuizAsync(Quizzes quizDto)
    {
        await _quizzes.InsertOneAsync(quizDto);
    }

    // Opdaterer en eksisterende quiz i databasen
    public async Task UpdateQuizAsync(Quizzes quizDto)
    {
        await _quizzes.ReplaceOneAsync(q => q._id == quizDto._id, quizDto);
    }

    // Sletter en quiz ud fra dens ID
    public async Task DeleteQuizAsync(string id)
    {
        await _quizzes.DeleteOneAsync(q => q._id == id);
    }
}