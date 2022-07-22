using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Quiz>> GetAllTeacherQuizzes();
    }
}
