using Modeller;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces;

// Interface for spørgsmål-specifikke databaseoperationer (Din "IQuestionRepository")
public interface IQuestion // OK, hvis du vil bruge dette navn som din repository interface
{
    Task<List<Question>> GetAllQuestionsAsync(); // Hent alle spørgsmål
    Task<Question> GetQuestionByIdAsync(string id); // Hent et enkelt spørgsmål ud fra ID (string!)
    Task CreateQuestionAsync(Question question); // Opret et nyt spørgsmål
    Task UpdateQuestionAsync(Question question); // Opdater et eksisterende spørgsmål
    Task DeleteQuestionAsync(string id); // Slet et spørgsmål ud fra ID
}