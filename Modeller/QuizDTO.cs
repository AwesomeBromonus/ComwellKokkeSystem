using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic; // Make sure this is included for List<T>

namespace Modeller;

public class Quizzes
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    public List<int> QuestionsIds { get; set; } = new List<int>(); // <--- VIGTIGT: Skal være List<int>
    public string CreatorUserId { get; set; } = string.Empty;
    public string CreatorName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
public class Question
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int Id { get; set; } 
    public string Text { get; set; } = string.Empty;
    public List<string> Options { get; set; } = new List<string>();
    public int CorrectAnswerIndex { get; set; }
}

public class QuizWithQuestions
{
    public Quizzes Quiz { get; set; }
    public List<Question> Questions { get; set; } = new List<Question>();
}

public class CreateQuizRequest
{
    public Quizzes Quiz { get; set; } 
    public List<Question> Questions { get; set; }
}