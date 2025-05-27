using Modeller; // Refererer til dine fælles datamodeller.
using MongoDB.Driver; // Nødvendig for at interagere med MongoDB.
using System.Collections.Generic;
using System.Threading.Tasks;
using ComwellSystemAPI.Interfaces; // Refererer til din IQuestion interface.
using System.Linq; // Nødvendig for .FirstOrDefault() og .Max() til ID-generering.

namespace ComwellSystemAPI.Repositories;
// Namespace: "Repositories" indeholder implementeringer for dataadgangslag.

/// <summary>
/// Implementerer <see cref="IQuestion"/> interfacet og håndterer alle databaseoperationer
/// relateret til <see cref="Question"/>-objekter i MongoDB.
/// </summary>
/// <remarks>
/// Fejlsøgningstip: Hvis du oplever problemer med MongoDB-forbindelsen her,
/// tjek din forbindelsesstreng i konstruktøren. Sørg for, at MongoDB-serveren kører,
/// og at netværksadgang er tilladt (f.eks. firewall, IP-whitelist i MongoDB Atlas).
/// </remarks>
public class QuestionRepository : IQuestion
{
    // En privat felt, der repræsenterer MongoDB-kollektionen for spørgsmål.
    // Typen IMongoCollection<Question> angiver, at denne kollektion gemmer Question-objekter.
    private readonly IMongoCollection<Question> _questions;

    /// <summary>
    /// Konstruktør for QuestionRepository. Ansvarlig for at etablere forbindelse til MongoDB.
    /// </summary>
    /// <remarks>
    /// UØDVENDIG KODE? Direkte instansiering af MongoClient her (new MongoClient(...))
    /// er ikke den bedste praksis for Dependency Injection (DI) i ASP.NET Core.
    /// Typisk vil MongoClient eller IMongoDatabase blive registreret som en singleton
    /// i dit `Program.cs` og derefter injiceret i repository-konstruktørerne.
    /// Dette gør konfigurationen mere centraliseret, testbar og genbrugelig.
    /// For at rette dette, se `ComwellSystemAPI/Program.cs` eksemplet fra tidligere.
    /// </remarks>
    public QuestionRepository()
    {
        // Opretter en ny MongoDB-klient med den angivne forbindelsesstreng.
        // ADVARSEL: Hardkodning af forbindelsesstrenge direkte i koden er DÅRLIG SIKKERHEDSPRAKSIS.
        // Flyt denne streng til 'appsettings.json' og læs den via IConfiguration.
        // Eksempel: var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString");
        var client = new MongoClient("mongodb+srv://Bromus:Bromus12344321@cluster0.k4kon.mongodb.net/");

        // Henter den specifikke database ("Comwell") fra MongoDB-klienten.
        var db = client.GetDatabase("Comwell");

        // Henter "Questions" kollektionen. MongoDB vil automatisk oprette denne kollektion,
        // første gang du indsætter et dokument i den.
        _questions = db.GetCollection<Question>("Questions");
    }

    /// <summary>
    /// Henter alle spørgsmål fra "Questions" kollektionen.
    /// </summary>
    /// <returns>En liste af alle Question-objekter.</returns>
    public async Task<List<Question>> GetAllQuestionsAsync()
    {
        // .Find(_ => true): Finder alle dokumenter i kollektionen.
        // .ToListAsync(): Konverterer resultatet til en liste asynkront.
        return await _questions.Find(_ => true).ToListAsync();
    }

    /// <summary>
    /// Henter et enkelt spørgsmål ud fra dets unikke ID.
    /// </summary>
    /// <param name="id">ID'et for det spørgsmål, der skal hentes.</param>
    /// <returns>Question-objektet, hvis fundet; ellers null.</returns>
    public async Task<Question> GetQuestionByIdAsync(int id)
    {
        // .Find(q => q.Id == id): Finder dokumenter hvor 'Id' feltet matcher det givne 'id'.
        // .FirstOrDefaultAsync(): Returnerer det første match eller null, hvis intet match findes.
        // FEJLSØGNINGSTIP: Hvis et spørgsmål ikke findes, tjek om ID'et er korrekt (case-sensitivt i nogle DB'er, men ikke her med int).
        // Brug MongoDB Compass til at verificere, om dokumentet med det specifikke ID eksisterer.
        return await _questions.Find(q => q.Id == id).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Opretter et nyt spørgsmål i "Questions" kollektionen.
    /// </summary>
    /// <param name="question">Det Question-objekt, der skal oprettes.</param>
    public async Task CreateQuestionAsync(Question question)
    {
        // Genererer et nyt, unikt ID til spørgsmålet, før det indsættes.
        // Dette er nødvendigt, fordi du bruger 'int' som ID, og MongoDB ikke automatisk genererer sekventielle int ID'er.
        question.Id = await GetNextQuestionIdAsync();
        // .InsertOneAsync(): Indsætter et enkelt dokument i kollektionen.
        await _questions.InsertOneAsync(question);
    }

    /// <summary>
    /// Opdaterer et eksisterende spørgsmål i "Questions" kollektionen.
    /// </summary>
    /// <param name="question">Det Question-objekt med opdaterede værdier. 'Id' bruges til at finde dokumentet.</param>
    public async Task UpdateQuestionAsync(Question question)
    {
        // .ReplaceOneAsync(filter, replacement): Finder et dokument baseret på filteret (q.Id == question.Id)
        // og erstatter det fuldstændigt med det nye 'question' objekt.
        // FEJLSØGNINGSTIP: Hvis en opdatering ikke virker, tjek om 'question.Id' er korrekt og om dokumentet eksisterer.
        // Kontroller også, om 'question' objektet indeholder de korrekte opdaterede værdier.
        await _questions.ReplaceOneAsync(q => q.Id == question.Id, question);
    }

    /// <summary>
    /// Sletter et spørgsmål fra "Questions" kollektionen ud fra dets ID.
    /// </summary>
    /// <param name="id">ID'et for det spørgsmål, der skal slettes.</param>
    public async Task DeleteQuestionAsync(int id)
    {
        // .DeleteOneAsync(): Sletter det første dokument, der matcher filteret.
        await _questions.DeleteOneAsync(q => q.Id == id);
    }

    /// <summary>
    /// Genererer det næste sekventielle ID for et nyt spørgsmål.
    /// Finder det højeste eksisterende 'Id' i kollektionen og tilføjer 1.
    /// </summary>
    /// <returns>Det næste ledige ID.</returns>
    /// <remarks>
    /// DATABASEDESIGN OVERVEJELSE / SIKKERHEDSADVARSEL:
    /// Denne metode kan have race condition problemer i et høj-konkurrence miljø.
    /// Hvis to brugere forsøger at oprette et spørgsmål samtidigt, kan de begge få det samme 'næste ID',
    /// hvilket vil resultere i en duplikat nøglefejl ved indsættelse.
    /// For at opnå garanteret unikhed og atomisk ID-generering i MongoDB, er det bedre at:
    /// 1. Bruge MongoDB's indbyggede `ObjectId` (ændre <see cref="Question.Id"/> til string og fjerne `BsonRepresentation(Int32)`).
    /// 2. Implementere en separat "counters" kollektion og bruge `findOneAndUpdate` med `$inc` operationen for at atomisk inkrementere et ID.
    /// </remarks>
    private async Task<int> GetNextQuestionIdAsync()
    {
        // Sorterer alle spørgsmål faldende efter deres ID for at finde det højeste ID.
        var sort = Builders<Question>.Sort.Descending(q => q.Id);
        // Finder kun det første (højeste ID) dokument og returnerer det.
        var lastQuestion = await _questions.Find(_ => true).Sort(sort).Limit(1).FirstOrDefaultAsync();
        // Hvis der ikke er nogen spørgsmål (kollektionen er tom), start med ID 1; ellers tag det sidste ID + 1.
        return lastQuestion == null ? 1 : lastQuestion.Id + 1;
    }
}