namespace Quizard.Interfaces
{
    public interface IQuizParserService
    {
        Task<string> GetQuizLMS(IFormFile file);
    }
}
