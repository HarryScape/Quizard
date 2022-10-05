using Quizard.Models;

namespace Quizard.ViewModels
{
    public class UpdateQuestionViewModel
    {
        public Question Question { get; set; }
        public IFormFile? file { get; set; }
    }
}
