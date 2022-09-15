using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IQuizParserService
    {
        Task<string> GetQuizType(IFormFile file);
        Task<CreateQuizViewModel> GenerateQuizViewModel(int id);
        Task<List<SelectListItem>> GenerateQuestionTypes();
        Task<TakeQuizViewModel> GenerateTakeQuizViewModel(int id);
    }
}
