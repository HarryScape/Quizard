using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IQuizRepository
    {
        // GET commands
        Task<IEnumerable<Quiz>> GetAll();
        //Task<Quiz> GetByIdAsync(int id);


        // CRUD's
        bool Add(Quiz quiz);
        bool Add(Question quiz);
        bool Add(Answer quiz);
        bool Update(Quiz quiz);
        bool Delete(Quiz quiz);
        bool Save();
    }
}

