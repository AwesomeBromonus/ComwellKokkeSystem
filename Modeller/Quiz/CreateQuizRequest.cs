// CreateQuizRequest.cs
using System.Collections.Generic;

namespace Modeller
{
    public class CreateQuizRequest
    {
        // Vigtigt: Quizzes og Question klasserne skal være tilgængelige
        // via 'using Modeller;' eller fuld namespace-kvalifikation.
        // Da de også er i Modeller namespace, er det fint.
        public Quizzes Quiz { get; set; } = new Quizzes(); // Initialiser for at undgå null-reference
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}