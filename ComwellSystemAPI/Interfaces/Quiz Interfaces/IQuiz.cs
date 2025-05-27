using Modeller; // Refererer til dine fælles datamodeller (specifikt Quizzes).
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces;
// Namespace: "Interfaces" definerer kontrakter for repositories, der beskriver,
// hvilke dataadgangsoperationer der er tilgængelige for quizzes.

/// <summary>
/// Interface, der definerer kontrakten for quiz-specifikke dataadgangsoperationer (repository-mønster).
/// </summary>
public interface IQuiz // Navnet er "IQuiz", men det fungerer som din "IQuizRepository".
{
    // Formål: Henter en liste af alle quizzes.
    // Returværdi: Et Task, der returnerer en liste af Quizzes-objekter.
    Task<List<Quizzes>> GetQuizzesAsync();

    // Formål: Henter en enkelt quiz baseret på dens integer ID.
    // Parameter: 'id' er den unikke identifikator for quizzen.
    // Returværdi: Et Task, der returnerer et Quizzes-objekt.
    Task<Quizzes> GetQuizByIdAsync(int id);

    // Formål: Opretter en ny quiz i databasen.
    // Parameter: 'quiz' er det Quizzes-objekt, der skal gemmes.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task CreateQuizAsync(Quizzes quiz);

    // Formål: Opdaterer en eksisterende quiz i databasen.
    // Parameter: 'quiz' er det Quizzes-objekt med opdaterede værdier.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task UpdateQuizAsync(Quizzes quiz);

    // Formål: Sletter en quiz fra databasen baseret på dens integer ID.
    // Parameter: 'id' er ID'et for den quiz, der skal slettes.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task DeleteQuizAsync(int id);
}