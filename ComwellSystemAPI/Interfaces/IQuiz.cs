using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces;

// Interface for quiz-specifikke databaseoperationer (Din "IQuizRepository")
public interface IQuiz // OK, hvis du vil bruge dette navn som din repository interface
{
    Task<List<Quizzes>> GetQuizzesAsync(); // Hent alle quizzes
    Task<Quizzes> GetQuizByIdAsync(int id); // Hent en enkelt quiz ud fra ID (string!)
    Task CreateQuizAsync(Quizzes quiz); // Opret en ny quiz
    Task UpdateQuizAsync(Quizzes quiz); // Opdater en eksisterende quiz
    Task DeleteQuizAsync(int id); // Slet en quiz ud fra ID
}