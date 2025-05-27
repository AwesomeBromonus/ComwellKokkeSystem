// Question.cs
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Modeller
{
    public class Question
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int Id { get; set; } 
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectAnswerIndex { get; set; }
    }
}