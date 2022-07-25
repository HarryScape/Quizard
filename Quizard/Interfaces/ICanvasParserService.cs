using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface ICanvasParserService
    {
        Task<bool> ParseQuiz(IFormFile file, DashboardViewModel dashboardViewModel);
    }
}
