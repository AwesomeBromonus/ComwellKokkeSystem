// ComwellKokkeSystem/Service/QuizService/IQuizService.cs
using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service.QuizService
{
    public interface IQuizService
    {
        Task<List<Quizzes>?> GetAllQuizzesAsync();

        Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(string quizId);

        Task CreateQuizAsync(CreateQuizRequest request);

        // RETTET: Denne skal også bruge 'Quizzes' for at matche implementeringen!
        Task UpdateQuizAsync(string quizId, Quizzes quizDto); // <--- RETTET TIL QUIZZES!

        Task DeleteQuizAsync(string quizId);
    }
}