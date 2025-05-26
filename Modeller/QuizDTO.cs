using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic; // Make sure this is included for List<T>

namespace Modeller;

public class Quizzes
{
    [BsonId]
    [BsonElement("_id")]
    public string _id { get; set; }
    public string Title { get; set; }
    public List<string> QuestionsIds { get; set; } = new List<string>();
    public string CreatorUserId { get; set; }
    public string CreatorName { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

public class Question
{
    // Specifies that this property is the MongoDB document's primary key (_id)
    [BsonId]
    // Specifies that this property maps to the "_id" element in MongoDB
    [BsonElement("_id")]
    public string _id { get; set; } // The unique identifier for the Question
    public string Text { get; set; } // The actual question text
    // A list of strings representing the possible answer options for the question
    public List<string> Options { get; set; } = new List<string>(); // Initialize to prevent null reference issues
    // The zero-based index of the correct answer within the Options list
    public int CorrectAnswerIndex { get; set; } // Renamed for clarity to distinguish from score or count
    
    // You might also want a field for the QuizId this question belongs to, though not strictly needed with references
    // public string QuizId { get; set; } // If you want to query questions by quiz directly
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