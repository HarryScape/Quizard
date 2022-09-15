using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IBlackboardParserService
    {
        Task<bool> ParseQuiz(IFormFile file);
    }
}
