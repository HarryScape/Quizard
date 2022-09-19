using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface ITakeQuizRepository
    {
        Task<IEnumerable<UserQuestionResponse>> GetResponsesbyAttemptId(int attemptId);
        Task<UserQuizAttempt> GetAttemptById(int id);
        Task<IEnumerable<UserQuestionResponse>> GetResponsesbyQuestion(int attemptId, int questionId);
        Task<UserQuestionResponse> GetSingleResponseByQuestion(int attemptId, int questionId);
        Task<IEnumerable<UserQuestionResponse>> GetSingleResponseByQuestionId(int questionId);
        Task<IEnumerable<UserQuizAttempt>> GetAttemptsByQuizId(int quizId);

        public bool Add(UserQuizAttempt userQuizAttempt);
        public bool Update(UserQuizAttempt userQuizAttempt);
        public bool Delete(UserQuizAttempt userQuizAttempt);
        public bool Add(UserQuestionResponse userQuestionResponse);
        public bool Update(UserQuestionResponse userQuestionResponse);
        public bool Delete(UserQuestionResponse userQuestionResponse);
    }
}
