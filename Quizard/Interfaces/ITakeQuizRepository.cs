using Quizard.Models;

namespace Quizard.Interfaces
{
    public interface ITakeQuizRepository
    {
        Task<IEnumerable<UserQuestionResponse>> GetResponsesbyAttemptId(int attemptId);
        Task<UserQuizAttempt> GetAttemptById(int id);

        public bool Add(UserQuizAttempt userQuizAttempt);
        public bool Update(UserQuizAttempt userQuizAttempt);
        public bool Delete(UserQuizAttempt userQuizAttempt);
        public bool Add(UserQuestionResponse userQuestionResponse);
        public bool Update(UserQuestionResponse userQuestionResponse);
        public bool Delete(UserQuestionResponse userQuestionResponse);
    }
}
