using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Mvc;
using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IQuizExportService
    {
        Task<ExportQuizViewModel> GenerateQuizViewModel(int id);
        Task<ActionResult> GenerateDocx(ExportQuizViewModel exportQuizViewModel);
        Task<string> GenerateQTI();

    }
}
