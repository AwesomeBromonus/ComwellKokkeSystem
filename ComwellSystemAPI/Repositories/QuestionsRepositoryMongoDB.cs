using Modeller;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComwellSystemAPI.Interfaces; // Bruger din IQuestion interface

namespace ComwellSystemAPI.Repositories;

// Implementering af IQuestion interfacet, der interagerer med MongoDB for Questions
public class QuestionRepository : IQuestion // Korrigeret: Implementerer nu IQuestion
{
    private readonly IMongoCollection<Question> _questions; // MongoDB collection til questions

    // Konstruktør: IMongoDatabase injiceres her fra Dependency Injection
    public QuestionRepository() // <--- Ingen parameter her
    {
        // Genindsæt manuel oprettelse af klient og database
        var client = new MongoClient("mongodb+srv://Bromus:Bromus12344321@cluster0.k4kon.mongodb.net/"); // Din egen forbindelsesstreng
        var db = client.GetDatabase("Comwell"); // Din egen database
        _questions = db.GetCollection<Question>("Questions");
    }

    // Henter alle spørgsmål fra databasen
    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _questions.Find(_ => true).ToListAsync();
    }

    // Henter et enkelt spørgsmål ud fra dens ID
    public async Task<Question> GetQuestionByIdAsync(string id)
    {
        return await _questions.Find(q => q._id == id).FirstOrDefaultAsync();
    }

    // Opretter et nyt spørgsmål i databasen
    public async Task CreateQuestionAsync(Question question)
    {
        await _questions.InsertOneAsync(question);
    }

    // Opdaterer et eksisterende spørgsmål i databasen
    public async Task UpdateQuestionAsync(Question question)
    {
        await _questions.ReplaceOneAsync(q => q._id == question._id, question);
    }

    // Sletter et spørgsmål ud fra dens ID
    public async Task DeleteQuestionAsync(string id)
    {
        await _questions.DeleteOneAsync(q => q._id == id);
    }
}