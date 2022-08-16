using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IModuleRepository
    {
        Task<List<Module>> GetUserModules();
        Task<Module> GetModuleById(int id);
        Task<IEnumerable<User>> GetStudentsByModule(int moduleId);

        public bool Save();
        public bool Add(Module module);
        public bool Update(Module module);
        public bool Update(User user);
        public bool Delete(Module module);
        public bool Add(UserModule userModule);
        public bool Update(UserModule userModule);
        public bool Delete(UserModule userModule);

    }
}
