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


        /// <summary>
        /// LINQ retrieves all quizzes for Admin user
        /// </summary>
        /// <returns>List of quizzes</returns>
        public async Task<IEnumerable<Quiz>> GetAll()
        {
            return await _context.Quizzes.ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves specific quiz by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Quiz object</returns>
        public async Task<Quiz> GetQuizById(int id)
        {
            return await _context.Quizzes.FirstOrDefaultAsync(i => i.Id == id);
        }


        /// <summary>
        /// Get quiz sections that belong to a quiz
        /// </summary>
        /// <param name="Quizid"></param>
        /// <returns>List of sections</returns>
        public async Task<IEnumerable<Section>> GetQuizSections(int Quizid)
        {
            return await _context.Sections.Where(c => c.Quiz.Id.Equals(Quizid)).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves a section by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Section object</returns>
        public async Task<Section> GetSectionById(int id)
        {
            return await _context.Sections.FirstOrDefaultAsync(i => i.Id == id);
        }


        /// <summary>
        /// Get all questions for Admin user
        /// </summary>
        /// <returns>List of questions</returns>
        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves all questions belonging to a quiz
        /// </summary>
        /// <param name="Quizid"></param>
        /// <returns>List of questions</returns>
        public async Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid)
        {
            return await _context.Questions.OrderBy(o => o.QuestionPosition).Where(i => i.SectionId == i.Section.Id).Where(j => j.Section.Quiz.Id == Quizid).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves questions by the section Id they belong to
        /// </summary>
        /// <param name="sectionId"></param>
        /// <returns>List of questions</returns>
        public async Task<IEnumerable<Question>> GetQuestionBySectionID(int sectionId)
        {
            return await _context.Questions.Where(i => i.SectionId == sectionId).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves a question by it's Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Question object</returns>
        public async Task<Question> GetQuestionById(int id)
        {
            return await _context.Questions.FirstOrDefaultAsync(i => i.Id == id);
        }


        /// <summary>
        /// LINQ Retrieves only questions that have child question parts
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns>List of questions</returns>
        public async Task<IEnumerable<Question>> GetParentQuestions(int quizId)
        {
            return await _context.Questions.Where(i => i.SectionId == i.Section.Id).Where(j => j.Section.Quiz.Id == quizId).Where(i => i.ParentId == null).ToListAsync();
        }

        /// <summary>
        /// LINQ Retrieves only questions that have a parent question / case study
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of questions</returns>
        public async Task<IEnumerable<Question>> GetChildQuestions(int id)
        {
            return await _context.Questions.Where(i => i.ParentId == id).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves all answers for admin user
        /// </summary>
        /// <returns>List of answers</returns>
        public async Task<IEnumerable<Answer>> GetAllAnswers()
        {
            return await _context.Answers.ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves answers belonging to a quiz
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns>List of answers</returns>
        public async Task<IEnumerable<Answer>> GetSpecificAnswers(int quizId)
        {
            return await _context.Answers.Where(i => i.QuestionId == i.Question.Id)
                .Where(j => j.Question.SectionId == j.Question.Section.Id)
                .Where(c => c.Question.Section.QuizId == quizId).ToListAsync();
        }


        /// <summary>
        /// LINQ retrieves answers belonging to a specific question
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List of answers</returns>
        public async Task<IEnumerable<Answer>> GetAnswersByQuestion(int id)
        {
            return await _context.Answers.Where(i => i.QuestionId == id).ToListAsync();
        }


        // CRUD operations
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

        public bool Add(Answer answer)
        {
            _context.Add(answer);
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
