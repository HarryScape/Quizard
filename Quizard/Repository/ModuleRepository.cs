using Microsoft.EntityFrameworkCore;
using Quizard.Data;
using Quizard.Interfaces;
using Quizard.Models;

namespace Quizard.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ModuleRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// LINQ retrieves modules belonging to the logged in teacher
        /// </summary>
        /// <returns>List of user modules</returns>
        public async Task<List<Module>> GetUserModules()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userModules = _context.Modules.Where(i => i.ModuleLeaderId == currentUser);
            return userModules.ToList();
        }


        /// <summary>
        /// LINQ retrieves a module by module Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Module object</returns>
        public async Task<Module> GetModuleById(int id)
        {
            return await _context.Modules.FirstOrDefaultAsync(i => i.Id == id);
        }


        /// <summary>
        /// LINQ retrieves students enrolled in a specific module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>List of users</returns>
        public async Task<IEnumerable<User>> GetStudentsByModule(int moduleId)
        {
            return await _context.Users.Where(i => i.Modules.Any(m => m.ModuleId == moduleId)).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves a UserModule for removing students from a module
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="moduleId"></param>
        /// <returns>UserModule object</returns>
        public async Task<UserModule> GetSpecificUserModule(string userId, int moduleId)
        {
            return await _context.UserModules.FirstOrDefaultAsync(i => i.UserId == userId && i.ModuleId == moduleId);
        }


        // CRUD operations
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Add(Module module)
        {
            _context.Add(module);
            return Save();
        }

        public bool Update(Module module)
        {
            _context.Update(module);
            return Save();
        }

        public bool Delete(Module module)
        {
            _context.Remove(module);
            return Save();
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool Add(UserModule userModule)
        {
            _context.Add(userModule);
            return Save();
        }

        public bool Update(UserModule userModule)
        {
            _context.Update(userModule);
            return Save();
        }

        public bool Delete(UserModule userModule)
        {
            _context.Remove(userModule);
            return Save();
        }
    }
}
