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

    public QuestionRepository(IMongoDatabase database)
    {
        this._questions = database.GetCollection<Question>("Questions");
    }

    public async Task<List<Question>> GetAllQuestionsAsync()
    {
       return await _questions.Find(_ => true).ToListAsync();
    }

    // RETTET: id parameter er nu int
    public async Task<Question> GetQuestionByIdAsync(int id)
    {
        return await _questions.Find(q => q.Id == id).FirstOrDefaultAsync(); // Sammenligner int med int
    }

    public async Task CreateQuestionAsync(Question question)
    {
        question.Id = await GetNextQuestionIdAsync(); // Generer ID her
        await _questions.InsertOneAsync(question);
    }

    // RETTET: question.Id er int, filter er på int. Fjernede `_id` reference.
    public async Task UpdateQuestionAsync(Question question)
    {
        await _questions.ReplaceOneAsync(q => q.Id == question.Id, question); // Sammenligner int med int
    }

    // RETTET: id parameter er nu int
    public async Task DeleteQuestionAsync(int id)
    {
        await _questions.DeleteOneAsync(q => q.Id == id); // Sammenligner int med int
    }

    // NY METODE: GetNextQuestionIdAsync - Denne hører kun til QuestionRepository
    private async Task<int> GetNextQuestionIdAsync()
    {
        var sort = Builders<Question>.Sort.Descending(q => q.Id);
        var lastQuestion = await _questions.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        return lastQuestion == null ? 1 : lastQuestion.Id + 1;
    }
    
}