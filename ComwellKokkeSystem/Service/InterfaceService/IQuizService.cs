// ComwellKokkeSystem/Service/QuizService/IQuizService.cs
using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;



    public interface IQuizService
    {
        Task<List<Quizzes>?> GetAllQuizzesAsync();

        Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(int quizId);

    Task<bool> CreateQuizAsync(CreateQuizRequest request);

    // RETTET: Denne skal også bruge 'Quizzes' for at matche implementeringen!
    Task UpdateQuizAsync(int quizId, Quizzes quizzes); // <--- RETTET TIL QUIZZES!

        Task DeleteQuizAsync(int quizId);
        Task<Modeller.Quizzes?> GetQuizByIdAsync(int id);

    }
