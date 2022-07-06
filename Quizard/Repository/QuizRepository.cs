﻿using Quizard.Data;
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


        // Get Question
        public async Task<IEnumerable<Question>> GetAllQuestions()
        {
            return await _context.Questions.ToListAsync();
        }
        public async Task<IEnumerable<Question>> GetQuestionByQuizID(int Quizid)
        {
            return await _context.Questions.Where(c => c.Quiz.Id.Equals(Quizid)).ToListAsync();
        }


        // Get Answer
        public async Task<IEnumerable<Answer>> GetAllAnswers()
        {
            return await _context.Answers.ToListAsync();
        }
        public async Task<IEnumerable<Answer>> GetSpecificAnswers(int QuizId)
        {
            return await _context.Answers.Where(i => i.QuestionId == i.Question.Id).Where(j => j.Question.QuizId == QuizId).ToListAsync();
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
