using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface IQuizRepository
    {
        // GET commands
        Task<IEnumerable<Quiz>> GetAll();
        Task<IEnumerable<Question>> GetAllQuestions();
        Task<IEnumerable<Answer>> GetAllAns();

        //test
        //Task<Question> QuestionByQuizID(int id);
        Task<Question> GetQuestionFromQuizIdAsync(int id);
        Task<Quiz> GetFullQuizById(int id);
        Task<IEnumerable<Answer>> AnswerTest(IEnumerable<Question> q);
        Task<IEnumerable<Answer>> GetSpecificAnswers(int QuizId);


        Task<Quiz> GetByQuizIdAsync(int id);
        Task<Quiz> GetByQuizIdTwo(int id);
        Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid);
        Task<IEnumerable<Answer>> GetAnswerByQuestionID(int QuestionId);



        // CRUD
        bool Add(Quiz quiz);
        bool Add(Question quiz);
        bool Add(List<Answer> answers);
        bool Update(Quiz quiz);
        bool Delete(Quiz quiz);
        bool Save();
    }
}

