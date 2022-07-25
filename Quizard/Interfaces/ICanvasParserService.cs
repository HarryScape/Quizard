namespace Quizard.Interfaces
{
    public interface ICanvasParserService
    {
        Task<bool> ParseQuiz(IFormFile file);
    }
}
