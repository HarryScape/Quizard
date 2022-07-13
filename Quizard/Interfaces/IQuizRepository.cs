using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IQuizRepository
    {
        // GET commands
        Task<IEnumerable<Quiz>> GetAll();
        Task<Quiz> GetQuizById(int id);
        Task<IEnumerable<Section>> GetQuizSections(int QuizId);
        Task<IEnumerable<Question>> GetAllQuestions();
        Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid);
        Task<IEnumerable<Answer>> GetAllAnswers();
        Task<IEnumerable<Answer>> GetSpecificAnswers(int QuizId);





        // CRUD
        bool Add(Quiz quiz);
        bool Add(Section section);
        bool Add(Question quiz);
        bool Add(List<Answer> answers);
        bool Update(Quiz quiz);
        bool Delete(Quiz quiz);
        bool Save();
    }
}

