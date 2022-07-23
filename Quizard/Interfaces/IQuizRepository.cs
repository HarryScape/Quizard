using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IQuizRepository
    {
        // GET commands
        // Quiz
        Task<IEnumerable<Quiz>> GetAll();
        Task<Quiz> GetQuizById(int id);
        // Section
        Task<IEnumerable<Section>> GetQuizSections(int QuizId);
        Task<Section> GetSectionById(int id);
        // Question
        Task<IEnumerable<Question>> GetAllQuestions();
        Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid);
        Task<Question> GetQuestionById(int id);
        // Answer
        Task<IEnumerable<Answer>> GetAllAnswers();
        Task<IEnumerable<Answer>> GetSpecificAnswers(int QuizId);


        // CRUD
        bool Add(Quiz quiz);
        bool Add(Section section);
        bool Add(Question quiz);
        bool Add(List<Answer> answers);
        bool Update(Quiz quiz);
        bool Update(Question question);
        bool Update(Section section);
        bool Delete(Quiz quiz);
        bool Delete(Section section);
        bool Delete(Question question);
        bool Delete(Answer answer);
        bool DeleteAns(IEnumerable<Answer> answers);
        bool DeleteQuestions(IEnumerable<Question> questions);
        bool DeleteSections(IEnumerable<Section> sections);

        bool Save();
    }
}

