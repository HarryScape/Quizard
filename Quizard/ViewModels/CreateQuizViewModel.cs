using Quizard.Data.Enum;
using Quizard.Models;

namespace Quizard.ViewModels
{
    public class CreateQuizViewModel
    {
        public IEnumerable<Quiz> QuizVM { get; set; }
        public IEnumerable<Question> QuestionVM { get; set; }
        public IEnumerable<Answer> AnswerVM { get; set; }
    }
}
