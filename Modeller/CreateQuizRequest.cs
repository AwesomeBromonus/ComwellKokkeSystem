namespace Modeller
{

    public class CreateQuizRequest
    {
        public Quizzes Quiz { get; set; }
        public List<Question> Questions { get; set; }
    }
}