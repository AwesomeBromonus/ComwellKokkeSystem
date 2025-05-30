using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces
{
    // @* KLASSE: Interface til håndtering af spørgsmål i systemet *@
    public interface IQuestion
    {
        // @* METODE: Hent alle spørgsmål *@
        Task<List<Question>> GetAllQuestionsAsync();

        // @* METODE: Hent et enkelt spørgsmål baseret på id *@
        Task<Question> GetQuestionByIdAsync(int id);

        // @* METODE: Opret et nyt spørgsmål *@
        Task CreateQuestionAsync(Question question);

        // @* METODE: Opdater et eksisterende spørgsmål *@
        Task UpdateQuestionAsync(Question question);

        // @* METODE: Slet et spørgsmål baseret på id *@
        Task DeleteQuestionAsync(int id);
    }
}
