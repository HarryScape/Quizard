using Quizard.Data;
using Quizard.Interfaces;
using Quizard.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace Quizard.Repository
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _context;

        public QuizRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        // GIMME THEM ALL
        public async Task<Quiz> GetFullQuizById(int id)
        {
            return await _context.Quizzes
                .Include(i => i.QuizQuestions)
                .ThenInclude(j => j.QuestionAnswers)
                .FirstOrDefaultAsync(i => i.Id == id);
        }


        // Get Quiz
        // Get All
        public async Task<IEnumerable<Quiz>> GetAll()
        {
            return await _context.Quizzes.ToListAsync();
        }
        // Get by ID
        public async Task<Quiz> GetByQuizIdAsync(int id)
        {
            //return await _context.Quizzes.Include(i => i.Id).FirstOrDefaultAsync();
            return await _context.Quizzes.Include(i => i.Id).FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Quiz> GetByQuizIdTwo(int id)
        {
            //return await _context.Quizzes.Include(i => i.Id).FirstOrDefaultAsync();
            return await _context.Quizzes.FirstOrDefaultAsync(i => i.Id == id);
        }





        // Get Question
        // Get All
        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }
        // Get by ID
        public async Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid)
        {
            return await _context.Questions.Where(c => c.Quiz.Id.Equals(Quizid)).ToListAsync();
        }
        // TODO: Get by ID methods for joins.
        public async Task<Question> GetQuestionFromQuizIdAsync(int id)
        {
            return await _context.Questions.Include(i => i.Id).FirstOrDefaultAsync(i => i.Id == id);
        }
        //public async Task<IEnumerable<Question>> QuestionByQuizID(int Quizid)
        //{
        //    return await _context.Quizzes.Where(c => c.Id.Equals(Quizid)).ToListAsync();
        //}




        // Get Answer
        public async Task<IEnumerable<Answer>> GetAllAns()
        {
            return await _context.Answers.ToListAsync();
        }
        public async Task<IEnumerable<Answer>> GetAnswerByQuestionID(int QuestionId)
        {
            return await _context.Answers.Where(c => c.Question.Id.Equals(QuestionId)).ToListAsync();
        }
        public async Task<Answer> GetAnswerFromQuestionIdAsync(int id)
        {
            return await _context.Answers.Include(i => i.Id).FirstOrDefaultAsync(i => i.Id == id);
        }






        // CRUD

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
            foreach (Answer answer in answers)
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
