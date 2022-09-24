using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IMarkdownParserService
    {
        Task<bool> ParseQuizA(IFormFile file);
        Task<bool> ParseQuizB(IFormFile file);
    }
}
