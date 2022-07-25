namespace Quizard.Interfaces
{
    public interface IMoodleParserService
    {
        Task<bool> ParseQuiz(IFormFile file);
    }
}
