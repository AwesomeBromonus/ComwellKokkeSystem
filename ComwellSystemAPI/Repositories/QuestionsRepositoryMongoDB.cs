using Modeller;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using ComwellSystemAPI.Interfaces;
using System.Linq; // Nødvendig for .FirstOrDefault() og .Max() i GetNextQuestionIdAsync

namespace ComwellSystemAPI.Repositories;

public class QuestionRepository : IQuestion
{
    private readonly IMongoCollection<Question> _questions;

    // Konstruktor initialiserer MongoDB-samlingen for spørgsmål
    public QuestionRepository(IMongoDatabase database)
    {
        _questions = database.GetCollection<Question>("Questions");
    }

    // Henter alle spørgsmål som en liste
    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        return await _questions.Find(_ => true).ToListAsync();
    }

    // Henter et enkelt spørgsmål baseret på id
    public async Task<Question> GetQuestionByIdAsync(int id)
    {
        // Finder det første spørgsmål der matcher det givne id
        return await _questions.Find(q => q.Id == id).FirstOrDefaultAsync();
    }

    // Opretter et nyt spørgsmål med automatisk genereret id
    public async Task CreateQuestionAsync(Question question)
    {
        // Genererer næste ledige id for spørgsmålet
        question.Id = await GetNextQuestionIdAsync();
        // Indsætter det nye spørgsmål i samlingen
        await _questions.InsertOneAsync(question);
    }

    // Opdaterer et eksisterende spørgsmål baseret på id
    public async Task UpdateQuestionAsync(Question question)
    {
        // Erstatter det eksisterende spørgsmål med den nye data
        await _questions.ReplaceOneAsync(q => q.Id == question.Id, question);
    }

    // Sletter et spørgsmål baseret på id
    public async Task DeleteQuestionAsync(int id)
    {
        // Finder og sletter spørgsmålet med det givne id
        await _questions.DeleteOneAsync(q => q.Id == id);
    }

    // Hjælpefunktion der finder næste ledige id ved at finde højeste eksisterende id og lægge 1 til
    private async Task<int> GetNextQuestionIdAsync()
    {
        var sort = Builders<Question>.Sort.Descending(q => q.Id);
        var lastQuestion = await _questions.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return lastQuestion == null ? 1 : lastQuestion.Id + 1;
    }
}
