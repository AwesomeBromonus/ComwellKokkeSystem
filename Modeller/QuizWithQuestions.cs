
namespace Modeller
{


    public class QuizWithQuestions
    {
        public Quizzes Quiz { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}