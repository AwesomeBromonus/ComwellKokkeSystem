// Quizzes.cs
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System; // For DateTime

namespace Modeller
{
    public class Quizzes
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public List<int> QuestionsIds { get; set; } = new List<int>();
        public string CreatorUserId { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}