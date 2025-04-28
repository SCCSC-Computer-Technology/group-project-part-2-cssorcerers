
namespace IQSport.Models.QuizModels
{
    public class F1QuizViewModel
    {
        public List<F1QuizQuestion> Questions { get; set; } = new List<F1QuizQuestion>();
        public List<string> Answers { get; set; } = new List<string>();
    }

    public class F1QuizQuestion
    {
        public string QuestionText { get; set; } // Question text (e.g., "Which drivers race for Red Bull?")
        public List<string> Options { get; set; } = new List<string>(); // Options like "Max Verstappen", "Sergio Perez"
        public string CorrectAnswer { get; set; } // The correct answer, such as "Max Verstappen, Sergio Perez"
    }
}
