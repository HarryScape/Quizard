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

        // Join between all three tables
        //public async Task<Quiz> GetFullQuizById(int id)
        //{
        //    return await _context.Quizzes
        //        .Include(i => i.QuizQuestions)
        //        .ThenInclude(j => j.QuestionAnswers)
        //        .FirstOrDefaultAsync(i => i.Id == id);
        //}

        // Get Quiz
        public async Task<IEnumerable<Quiz>> GetAll()
        {
            return await _context.Quizzes.ToListAsync();
        }
        public async Task<Quiz> GetQuizById(int id)
        {
            return await _context.Quizzes.FirstOrDefaultAsync(i => i.Id == id);
        }


        // Get Sections
        public async Task<IEnumerable<Section>> GetQuizSections(int Quizid)
        {
            return await _context.Sections.Where(c => c.Quiz.Id.Equals(Quizid)).ToListAsync();
        }
        public async Task<Section> GetSectionById(int id)
        {
            return await _context.Sections.FirstOrDefaultAsync(i => i.Id == id);
        }


        // Get Question
        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }
        public async Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid)
        {
            //return await _context.Questions.Where(c => c.Quiz.Id.Equals(Quizid)).ToListAsync();
            return await _context.Questions.OrderBy(o => o.QuestionPosition).Where(i => i.SectionId == i.Section.Id).Where(j => j.Section.Quiz.Id == Quizid).ToListAsync();
        }
        public async Task<IEnumerable<Question>> GetQuestionBySectionID(int sectionId)
        {
            return await _context.Questions.Where(i => i.SectionId == sectionId).ToListAsync();
        }
        public async Task<Question> GetQuestionById(int id)
        {
            return await _context.Questions.FirstOrDefaultAsync(i => i.Id == id);
        }


        // Get Answer
        public async Task<IEnumerable<Answer>> GetAllAnswers()
        {
            return await _context.Answers.ToListAsync();
        }
        public async Task<IEnumerable<Answer>> GetSpecificAnswers(int QuizId)
        {
            //return await _context.Answers.Where(i => i.QuestionId == i.Question.Id).Where(j => j.Question.QuizId == QuizId).ToListAsync();
            return await _context.Answers.Where(i => i.QuestionId == i.Question.Id)
                .Where(j => j.Question.SectionId == j.Question.Section.Id)
                .Where(c => c.Question.Section.QuizId == QuizId).ToListAsync();
        }




        // CRUD

        public bool Add(Quiz quiz)
        {
            _context.Add(quiz);
            return Save();
        }

        public bool Add(Section section)
        {
            _context.Add(section);
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

        public bool Delete(Section section)
        {
            _context.Remove(section);
            return Save();
        }

        public bool Delete(Question question)
        {
            _context.Remove(question);
            return Save();
        }

        public bool Delete(Answer answer)
        {
            _context.Remove(answer);
            return Save();
        }




        public bool DeleteAns(IEnumerable<Answer> answers)
        {
            if (answers != null)
            {
                foreach (Answer answer in answers)
                {
                    _context.Remove(answer);
                }
            }
            return Save();
        }
        public bool DeleteQuestions(IEnumerable<Question> questions)
        {
            if (questions != null)
            {
                foreach (Question question in questions)
                {
                    _context.Remove(question);
                }
            }
            return Save();
        }
        public bool DeleteSections(IEnumerable<Section> sections)
        {
            if (sections != null)
            {
                foreach (Section section in sections)
                {
                    _context.Remove(section);
                }
            }
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

        public bool Update(Question question)
        {
            _context.Update(question);
            return Save();
        }

        public bool Update(Section section)
        {
            _context.Update(section);
            return Save();
        }
    }
}
