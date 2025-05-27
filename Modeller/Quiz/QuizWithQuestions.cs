using System.Collections.Generic;

namespace Modeller;
// Namespace: "Modeller" indeholder fælles datamodeller.

/// <summary>
/// Datatransferobjekt (DTO) brugt til at returnere en quiz sammen med alle dens tilknyttede spørgsmål.
/// Dette objekt samler data fra flere MongoDB collections (Quizzes og Questions) til én logisk enhed
/// for præsentation på klienten. Det er et eksempel på "aggregation" i applikationslaget.
/// </summary>
public class QuizWithQuestions
{
    // Egenskab: Quiz-objektet.
    // Initialiseres for at undgå null-referencer.
    public Quizzes Quiz { get; set; } = new Quizzes();

    // Egenskab: En liste af Question-objekter, der hører til quizzen.
    // Disse spørgsmål hentes separat og populeres baseret på ID'erne i Quiz.QuestionsIds.
    // Initialiseres til en tom liste.
    public List<Question> Questions { get; set; } = new List<Question>();
}