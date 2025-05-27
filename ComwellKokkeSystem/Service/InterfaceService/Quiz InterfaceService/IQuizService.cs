using Modeller; // Refererer til dine fælles datamodeller.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComwellKokkeSystem.Service.QuizService;
// Namespace: "Service.QuizService" indeholder interfaces for klient-side services.
// Disse interfaces definerer kontrakten for, hvad frontend kan gøre med quiz-data,
// uden at kende de underliggende HTTP-implementationsdetaljer.

/// <summary>
/// Interface, der definerer kontrakten for klient-side quiz-relaterede operationer.
/// </summary>
/// <remarks>
/// SYSTEMDESIGN: Dette interface er en del af "Ports and Adapters" (Hexagonal) arkitektur,
/// hvor det definerer en "port" for quiz-funktionalitet. `QuizService` klassen er "adapteren",
/// der implementerer denne port ved at kommunikere med det eksterne API.
/// Dette adskiller UI-logikken fra API-kommunikationslogikken.
/// </remarks>
public interface IQuizService
{
    // Formål: Henter alle quizzes.
    // Returværdi: En Task, der returnerer en liste af Quizzes-objekter (kan være null).
    Task<List<Quizzes>?> GetAllQuizzesAsync();

    // Formål: Henter en specifik quiz med dens tilknyttede spørgsmål.
    // Parameter: 'quizId' er ID'et for quizzen.
    // Returværdi: En Task, der returnerer et QuizWithQuestions-objekt (kan være null).
    Task<QuizWithQuestions?> GetQuizWithQuestionsAsync(int quizId);

    // Formål: Opretter en ny quiz med dens spørgsmål.
    // Parameter: 'request' er DTO'en, der indeholder quiz- og spørgsmålsdata.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task CreateQuizAsync(CreateQuizRequest request);

    // Formål: Opdaterer en eksisterende quiz.
    // Parameter: 'quizId' er ID'et for quizzen.
    // Parameter: 'quizzes' er objektet med de opdaterede quizdata.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    // UØDVENDIG KODE? Navnet 'quizzes' (flertal) som parameter for et enkelt objekt er lidt forvirrende.
    // Overvej at kalde den 'quiz' for at matche semantikken af et enkelt objekt.
    Task UpdateQuizAsync(int quizId, Quizzes quizzes);

    // Formål: Sletter en quiz.
    // Parameter: 'quizId' er ID'et for quizzen.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task DeleteQuizAsync(int quizId);

    // UØDVENDIG KODE? Som nævnt i QuizService.cs er denne metode muligvis unødvendig,
    // da GetQuizWithQuestionsAsync allerede giver en mere komplet visning af quizzen.
    // Overvej at fjerne denne, hvis den ikke har et særskilt UI-behov.
    Task<Modeller.Quizzes?> GetQuizByIdAsync(int id);
}