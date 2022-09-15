using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>> GetAll();
        Task<Quiz> GetQuizById(int id);
        Task<IEnumerable<Section>> GetQuizSections(int quizId);
        Task<Section> GetSectionById(int id);
        Task<IEnumerable<Question>> GetAllQuestions();
        Task<IEnumerable<Question>> GetQuestionByQuizID(int quizId);
        Task<IEnumerable<Question>> GetQuestionBySectionID(int sectionId);
        Task<IEnumerable<Question>> GetParentQuestions(int quizId);
        Task<IEnumerable<Question>> GetChildQuestions(int id);
        Task<Question> GetQuestionById(int id);
        Task<IEnumerable<Answer>> GetAllAnswers();
        Task<IEnumerable<Answer>> GetSpecificAnswers(int quizId);
        Task<IEnumerable<Answer>> GetAnswersByQuestion(int id);

        bool Add(Quiz quiz);
        bool Add(Section section);
        bool Add(Question quiz);
        bool Add(Answer answer);
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

