using Microsoft.EntityFrameworkCore;
using Quizard.Data;
using Quizard.Interfaces;
using Quizard.Models;

namespace Quizard.Repository
{
    public class TakeQuizRepository : ITakeQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public TakeQuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// LINQ retrieve question responses by the quiz attempt they belong to
        /// </summary>
        /// <param name="attemptId"></param>
        /// <returns>List of responses</returns>
        public async Task<IEnumerable<UserQuestionResponse>> GetResponsesbyAttemptId(int attemptId)
        {
            return await _context.UserQuestionResponses.Where(i => i.UserQuizAttemptId.Equals(attemptId)).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieve a quiz attempt by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>UserQuizAttempt object</returns>
        public async Task<UserQuizAttempt> GetAttemptById(int id)
        {
            return await _context.UserQuizAttempts.FirstOrDefaultAsync(i => i.Id == id);
        }


        /// <summary>
        /// LINQ retrieve responses by attempt and question Id
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="questionId"></param>
        /// <returns>List of UserQuestionResponse</returns>
        public async Task<IEnumerable<UserQuestionResponse>> GetResponsesbyQuestion(int attemptId, int questionId)
        {
            return await _context.UserQuestionResponses.Where(i => i.UserQuizAttemptId.Equals(attemptId)).Where(j => j.QuestionId.Equals(questionId)).ToListAsync();

        }


        /// <summary>
        /// LINQ retrieve a single response by question Id
        /// </summary>
        /// <param name="attemptId"></param>
        /// <param name="questionId"></param>
        /// <returns>UserQuestionResponse object</returns>
        public async Task<UserQuestionResponse> GetSingleResponseByQuestion(int attemptId, int questionId)
        {
            return await _context.UserQuestionResponses.FirstOrDefaultAsync(i => i.UserQuizAttemptId == attemptId && i.QuestionId == questionId);
        }



        //CRUD operations
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Add(UserQuizAttempt userQuizAttempt)
        {
            _context.Add(userQuizAttempt);
            return Save();
        }
        public bool Update(UserQuizAttempt userQuizAttempt)
        {
            _context.Update(userQuizAttempt);
            return Save();
        }
        public bool Delete(UserQuizAttempt userQuizAttempt)
        {
            _context.Remove(userQuizAttempt);
            return Save();
        }

        public bool Add(UserQuestionResponse userQuestionResponse)
        {
            _context.Add(userQuestionResponse);
            return Save();
        }
        public bool Update(UserQuestionResponse userQuestionResponse)
        {
            _context.Update(userQuestionResponse);
            return Save();
        }
        public bool Delete(UserQuestionResponse userQuestionResponse)
        {
            _context.Remove(userQuestionResponse);
            return Save();
        }
    }
}
