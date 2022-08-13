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
            var userModules = _context.Modules.Where(i => i.ModuleLeader.Id == currentUser);
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
    }
}
