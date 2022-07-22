using Quizard.Models;

namespace Quizard.ViewModels
{
    public class DashboardViewModel
    {
        public List<Quiz> Quizzes { get; set; }
        public string UserId { get; set; }
    }
}
