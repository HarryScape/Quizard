using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IQuizExportService
    {
        Task<ExportQuizViewModel> GenerateQuizViewModel(int id);
        public void GenerateDocx(ExportQuizViewModel exportQuizViewModel);

    }
}
