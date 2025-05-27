using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Modeller;
// Namespace: "Modeller" indeholder fælles datamodeller.

/// <summary>
/// Repræsenterer et enkelt spørgsmål i en quiz.
/// Bruges både som en datamodel for MongoDB og som en DTO mellem lag.
/// </summary>
public class Question
{
    // BsonId: Angiver, at denne egenskab er den primære nøgle (_id) i MongoDB-dokumentet.
    // BsonRepresentation(MongoDB.Bson.BsonType.Int32): Fortæller MongoDB-driveren, at 'Id' skal gemmes
    // som en 32-bit integer i databasen, selvom MongoDB's standard _id ofte er ObjectId.
    // Dette valg kræver, at du manuelt genererer unikke int ID'er i din applikationslogik.
    // SYSTEMDESIGN OVERVEJELSE: At bruge 'int' som ID i MongoDB kan være udfordrende, da det kræver
    // applikationsspecifik logik for at sikre unikke og sekventielle ID'er (som du gør i GetNextQuestionIdAsync).
    // MongoDBs standard ObjectId er designet til at være unikt distribueret og automatisk genereret.
    // Hvis du ikke har et stærkt krav om sekventielle int ID'er, er det ofte simplere at bruge:
    // [BsonId]
    // [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    // public string Id { get; set; } // Ændrer typen til string
    public int Id { get; set; }

    // Spørgsmålsteksten (f.eks. "Hvad er hovedstaden i Frankrig?").
    // Initialiseres til tom streng for at undgå null-referencer.
    public string Text { get; set; } = string.Empty;

    // En liste af mulige svaralternativer til spørgsmålet.
    // Initialiseres til en tom liste.
    public List<string> Options { get; set; } = new List<string>();

    // Indekset for det korrekte svar i 'Options' listen (0-baseret).
    // F.eks. hvis Options = ["Paris", "Berlin", "Rom"] og CorrectAnswerIndex = 0, så er "Paris" det korrekte svar.
    public int CorrectAnswerIndex { get; set; }
}