using Quizard.Models;

namespace Quizard.ViewModels
{
    public class TakeQuestionViewModel
    {
        public IEnumerable<UserQuestionResponse>? QuestionResponses { get; set; }
        public IEnumerable<Question>? Questions { get; set; }
    }
}
