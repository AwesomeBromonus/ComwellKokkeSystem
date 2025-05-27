using System.Collections.Generic;

namespace Modeller;
// Namespace: "Modeller" indeholder fælles datamodeller, der bruges på tværs af både frontend (Blazor WASM) og backend (ASP.NET Core API).

/// <summary>
/// Datatransferobjekt (DTO) brugt til at sende data for oprettelse af en ny quiz fra klienten til API'en.
/// Dette sikrer, at quiz- og spørgsmålsdata pakkes korrekt sammen til ét API-kald.
/// </summary>
public class CreateQuizRequest
{
    // Egenskab: Quiz-objektet, der skal oprettes.
    // Initialiseres her for at undgå null-referencer, når et nyt CreateQuizRequest-objekt instansieres.
    // Dette er god praksis for at undgå NullReferenceException ved senere adgang til .Quiz.
    public Quizzes Quiz { get; set; } = new Quizzes();

    // Egenskab: En liste af Question-objekter tilknyttet den nye quiz.
    // Hvert spørgsmål vil blive oprettet separat i databasen og derefter linket til quizzen via ID'er.
    // Initialiseres her for at undgå null-referencer.
    public List<Question> Questions { get; set; } = new List<Question>();
}