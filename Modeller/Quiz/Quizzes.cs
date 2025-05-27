using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System; // For DateTime

namespace Modeller;
// Namespace: "Modeller" indeholder fælles datamodeller.
// Bemærk: Klassen hedder 'Quizzes' (flertal), mens en enkelt instans repræsenterer én quiz.
// Det er mere konventionelt at navngive enkelte entiteter i ental (f.eks. 'Quiz' i stedet for 'Quizzes').
// Overvej at omdøbe denne klasse til 'Quiz' for bedre klarhed.

/// <summary>
/// Repræsenterer en enkelt quiz. Bruges både som en datamodel for MongoDB og som en DTO.
/// </summary>
public class Quizzes
{
    // BsonId: Angiver, at denne egenskab er den primære nøgle (_id) i MongoDB-dokumentet.
    // BsonRepresentation(MongoDB.Bson.BsonType.Int32): Fortæller MongoDB-driveren, at 'Id' skal gemmes
    // som en 32-bit integer i databasen. Ligesom med Question.Id kræver dette manuel ID-generering.
    public int Id { get; set; }

    // Titlen på quizzen (f.eks. "Matematik Quiz", "Historie om Danmark").
    public string Title { get; set; } = string.Empty;

    // En liste af ID'er for de spørgsmål, der tilhører denne quiz.
    // Dette er et eksempel på "referencer" i NoSQL (MongoDB), hvor du gemmer ID'er
    // til andre dokumenter i stedet for at bruge traditionelle relationer.
    // Listen initialiseres til en tom liste.
    public List<int> QuestionsIds { get; set; } = new List<int>();

    // ID'et for den bruger, der har oprettet quizzen.
    // Bruges til autorisation (f.eks. kun skaberen må redigere/slette).
    public string CreatorUserId { get; set; } = string.Empty;

    // Navnet på brugeren, der har oprettet quizzen. Kan bruges til visning.
    public string CreatorName { get; set; } = string.Empty;

    // Tidspunktet hvor quizzen blev oprettet. Sættes til den aktuelle UTC-tid ved instansiering.
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}