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
        public async Task<IEnumerable<UserQuestionResponse>> GetResponsesbyAttemptId(int attemptId)
        {
            return await _context.UserQuestionResponses.Where(i => i.UserQuizAttemptId.Equals(attemptId)).ToListAsync();
        }

        public async Task<UserQuizAttempt> GetAttemptById(int id)
        {
            return await _context.UserQuizAttempts.FirstOrDefaultAsync(i => i.Id == id);
        }

        // get response by question id and attempt id. Maybe not can do it in the controller...
        public async Task<IEnumerable<UserQuestionResponse>> GetResponsesbyQuestion(int attemptId, int questionId)
        {
            return await _context.UserQuestionResponses.Where(i => i.UserQuizAttemptId.Equals(attemptId)).Where(j => j.QuestionId.Equals(questionId)).ToListAsync();

        }

        // get single response
        public async Task<UserQuestionResponse> GetSingleResponseByQuestion(int attemptId, int questionId)
        {
            return await _context.UserQuestionResponses.FirstOrDefaultAsync(i => i.UserQuizAttemptId == attemptId && i.QuestionId == questionId);
        }



        // CRUD
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        // Quiz Attempt
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

        // Question Response
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
