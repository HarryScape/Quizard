using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetUserModules();
        Task<Module> GetModuleById(int id);

        public bool Save();
        public bool Add(Module module);
        public bool Update(Module module);
        public bool Delete(Module module);

    }
}
