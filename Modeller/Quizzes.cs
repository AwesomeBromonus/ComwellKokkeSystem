
namespace Modeller
{

    public class Quizzes
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<int> QuestionsIds { get; set; } = new List<int>(); // <--- VIGTIGT: Skal være List<int>
        public string CreatorUserId { get; set; } = string.Empty;
        public string CreatorName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}