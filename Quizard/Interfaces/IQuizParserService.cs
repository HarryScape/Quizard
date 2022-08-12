using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IQuizParserService
    {
        Task<string> GetQuizLMS(IFormFile file);
        Task<CreateQuizViewModel> GenerateQuizViewModel(int id);
    }
}
