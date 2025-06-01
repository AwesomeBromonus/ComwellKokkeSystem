using ComwellSystemAPI.Interfaces;
using Modeller;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ComwellSystemAPI.Repositories;

public class QuizRepositoryMongoDB : IQuiz
{
    private readonly IMongoCollection<Quizzes> _quizzes;

    public QuizRepositoryMongoDB(IMongoDatabase database)
    {
        _quizzes = database.GetCollection<Quizzes>("Quizzes");
        Console.WriteLine("✅ Forbindelse til MongoDB gennem DI etableret.");
    }
    public async Task<List<Quizzes>> GetQuizzesAsync()
    {
        try
        {
            var quizzes = await _quizzes.Find(_ => true).ToListAsync();
            Console.WriteLine($"Hentede {quizzes.Count} quizzer fra databasen.");
            return quizzes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved hentning af quizzer: {ex.Message}");
            throw;
        }
    }

    public async Task<Quizzes> GetQuizByIdAsync(int id)
    {
        try
        {
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

    public async Task CreateQuizAsync(Quizzes quiz)
    {
        try
        {
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

    public async Task UpdateQuizAsync(Quizzes quizDto)
    {
        try
        {
            await _quizzes.ReplaceOneAsync(q => q.Id == quizDto.Id, quizDto);
            Console.WriteLine($"Quiz med ID {quizDto.Id} blev opdateret.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved opdatering af quiz med ID {quizDto.Id}: {ex.Message}");
            throw;
        }
    }

    private async Task<int> GetNextQuizIdAsync()
    {
        try
        {
            var sort = Builders<Quizzes>.Sort.Descending(q => q.Id);
            var lastQuiz = await _quizzes.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
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

    public async Task DeleteQuizAsync(int id)
    {
        try
        {
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