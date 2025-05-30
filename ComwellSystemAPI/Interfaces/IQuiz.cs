using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface til håndtering af quizzes i systemet *@
    public interface IQuiz
    {
        // @* METODE: Hent alle quizzes *@
        Task<List<Quizzes>> GetQuizzesAsync();

        // @* METODE: Hent en enkelt quiz baseret på id *@
        Task<Quizzes> GetQuizByIdAsync(int id);

        // @* METODE: Opret en ny quiz *@
        Task CreateQuizAsync(Quizzes quiz);

        // @* METODE: Opdater en eksisterende quiz *@
        Task UpdateQuizAsync(Quizzes quiz);

        // @* METODE: Slet en quiz baseret på id *@
        Task DeleteQuizAsync(int id);
    }
}
