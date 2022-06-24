using Quizard.Data;
using Quizard.Interfaces;
using Quizard.Models;
using Microsoft.EntityFrameworkCore;


namespace Quizard.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public QuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Quiz quiz)
        {
            _context.Add(quiz);
            return Save();
        }

        public bool Add(Question question)
        {
            _context.Add(question);
            return Save();
        }

        public bool Add(List<Answer> answers)
        {
            foreach(Answer answer in answers)
            {
                _context.Add(answer);
            }
            return Save();
        }

        public bool Delete(Quiz quiz)
        {
            _context.Remove(quiz);
            return Save();
        }

        public async Task<IEnumerable<Quiz>> GetAll()
        {
            return await _context.Quizzes.ToListAsync();
        }

        //public Task<Quiz> GetByIdAsync(int id)
        //{
        //    return await _context.Quizzes.Include(i => i.QuizQuestions).FirstOrDefaultAsync();
        //}

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Quiz quiz)
        {
            _context.Update(quiz);
            return Save();
        }
    }
}
