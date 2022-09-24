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

        /// <summary>
        /// LINQ retrieves logged in teacher quizzes 
        /// </summary>
        /// <returns>List of user quizzes</returns>
        public async Task<List<Quiz>> GetAllTeacherQuizzes()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userQuizes = _context.Quizzes.OrderByDescending(j => j.DateCreated).Where(i => i.User.Id == currentUser);
            return userQuizes.ToList();
        }

        /// <summary>
        /// LINQ retrieves logged in student quizzes
        /// </summary>
        /// <returns>List of user quizzes</returns>
        public async Task<List<Quiz>> GetAllStudentQuizzes()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var studentModules = _context.UserModules.Where(i => i.UserId == currentUser);
            var studentQuizzes = _context.Quizzes.OrderByDescending(j => j.DateCreated).Where(i => i.Deployed && studentModules.Any(m => m.ModuleId == i.ModuleId));

            return studentQuizzes.ToList();
        }

    }
}
