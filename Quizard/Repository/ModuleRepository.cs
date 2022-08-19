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


        public async Task<List<Module>> GetUserModules()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            //var userModules = _context.Modules.Where(i => i.ModuleLeader.Id == currentUser);
            var userModules = _context.Modules.Where(i => i.ModuleLeaderId == currentUser);
            return userModules.ToList();
        }

        public async Task<Module> GetModuleById(int id)
        {
            return await _context.Modules.FirstOrDefaultAsync(i => i.Id == id);
        }

        //public async Task<Module> GetModuleByUser(string id)
        //{
        //    return await _context.Modules.FirstOrDefaultAsync(i => i.Id = id);
        //}

        public async Task<IEnumerable<User>> GetStudentsByModule(int moduleId)
        {
            return await _context.Users.Where(i => i.Modules.Any(m => m.ModuleId == moduleId)).ToListAsync();
        }

        public async Task<UserModule> GetSpecificUserModule(string userId, int moduleId)
        {
            return await _context.UserModules.FirstOrDefaultAsync(i => i.UserId == userId && i.ModuleId == moduleId);
            //return await _context.
            //return null;
        }



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
