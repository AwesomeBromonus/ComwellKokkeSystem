using Modeller; // Refererer til dine fælles datamodeller.
using System.Collections.Generic; // Ikke direkte brugt i de eksisterende metoder, men ofte relevant.
using System.Threading.Tasks;

namespace ComwellSystemAPI.Interfaces;
// Namespace: "Interfaces" definerer kontrakter for repositories, der beskriver,
// hvilke dataadgangsoperationer der er tilgængelige for spørgsmål.

/// <summary>
/// Interface, der definerer kontrakten for spørgsmåls-specifikke dataadgangsoperationer (repository-mønster).
/// </summary>
public interface IQuestion // Navnet er "IQuestion", men det fungerer som din "IQuestionRepository".
{
    // Formål: Henter et enkelt spørgsmål baseret på dets integer ID.
    // Parameter: 'id' er den unikke identifikator for spørgsmålet.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation, og som returnerer et Question-objekt.
    // FEJLSØGNINGSTIP: Hvis metoden returnerer null, betyder det, at et spørgsmål med det givne ID ikke blev fundet.
    Task<Question> GetQuestionByIdAsync(int id);

    // Formål: Opretter et nyt spørgsmål i databasen.
    // Parameter: 'question' er det Question-objekt, der skal gemmes.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task CreateQuestionAsync(Question question);

    // Formål: Sletter et spørgsmål fra databasen baseret på dets integer ID.
    // Parameter: 'id' er ID'et for det spørgsmål, der skal slettes.
    // Returværdi: Et Task, der repræsenterer den asynkrone operation.
    Task DeleteQuestionAsync(int id);

    // UØDVENDIG KODE / MANGLENDE FUNKTIONALITET?
    // Du har GetAllQuestionsAsync og UpdateQuestionAsync i QuestionRepository,
    // men de er ikke defineret i dette interface. For at overholde interface-kontrakten,
    // skal alle offentlige metoder, der forventes at være tilgængelige via interfacet,
    // også være defineret her. Hvis disse metoder ikke skal bruges af andre dele af din applikation
    // (f.eks. din QuizService), kan de undlades fra interfacet. Men hvis de skal bruges, skal de tilføjes.
    // Eksempel på tilføjelser:
    // Task<List<Question>> GetAllQuestionsAsync();
    // Task UpdateQuestionAsync(Question question);
}