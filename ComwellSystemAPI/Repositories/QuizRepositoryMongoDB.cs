using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq; // Nødvendig for .FirstOrDefault() og .Max() i GetNextQuizIdAsync

namespace ComwellSystemAPI.Repositories;

public class QuizRepositoryMongoDB : IQuiz
{
    private readonly IMongoCollection<Quizzes> _quizzes;

    public QuizRepositoryMongoDB()
    {
        var client = new MongoClient("mongodb+srv://Bromus:Bromus12344321@cluster0.k4kon.mongodb.net/");
        var db = client.GetDatabase("Comwell");
        _quizzes = db.GetCollection<Quizzes>("Quizzes");
    }

    public async Task<List<Quizzes>> GetQuizzesAsync()
    {
        return await _quizzes.Find(_ => true).ToListAsync();
    }
    
    // RETTET: id parameter er nu int
    public async Task<Quizzes> GetQuizByIdAsync(int id)
    {
        return await _quizzes.Find(q => q.Id == id).FirstOrDefaultAsync(); // Sammenligner int med int
    }
    
    public async Task CreateQuizAsync(Quizzes quiz) // Parameter er quiz (ikke quizDto)
    {
        quiz.Id = await GetNextQuizIdAsync(); // Generer ID her
        await _quizzes.InsertOneAsync(quiz);
    }

    // RETTET: quizDto.Id er int, filter er på int
    public async Task UpdateQuizAsync(Quizzes quizDto)
    {
        await _quizzes.ReplaceOneAsync(q => q.Id == quizDto.Id, quizDto); // Sammenligner int med int
    }

    // Korrekt GetNextQuizIdAsync metode
    private async Task<int> GetNextQuizIdAsync()
    {
        var sort = Builders<Quizzes>.Sort.Descending(q => q.Id);
        var lastQuiz = await _quizzes.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return lastQuiz == null ? 1 : lastQuiz.Id + 1;
    }
    
    // RETTET: id parameter er nu int
    public async Task DeleteQuizAsync(int id)
    {
        await _quizzes.DeleteOneAsync(q => q.Id == id); // Sammenligner int med int
    }
}