using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IMoodleParserService
    {
        Task<bool> ParseQuiz(IFormFile file, DashboardViewModel dashboardViewModel);
    }
}
