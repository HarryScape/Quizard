using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Mvc;
using Quizard.ViewModels;

namespace Quizard.Interfaces
{
    public interface IQuizExportService
    {
        Task<ExportQuizViewModel> GenerateQuizViewModel(int id);
        public byte[] GenerateDocx(ExportQuizViewModel exportQuizViewModel);

    }
}
