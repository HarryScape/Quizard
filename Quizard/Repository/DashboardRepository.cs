using Quizard.Data;
using Quizard.Interfaces;
using Quizard.Models;

namespace Quizard.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Quiz>> GetAllTeacherQuizzes()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userQuizes = _context.Quizzes.Where(i => i.User.Id == currentUser);
            return userQuizes.ToList();
        }

        // TODO: return quizzes for student module.
        // public async Task<List<Quiz>> GetAllStudentQuizzes()
    }
}
