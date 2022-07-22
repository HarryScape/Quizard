﻿using Microsoft.AspNetCore.Mvc;
using Quizard.Data;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.Repository;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IDashboardRepository _dashboardRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DashboardController(IDashboardRepository dashboardRepository, IQuizRepository quizRepository, IHttpContextAccessor contextAccessor)
        {
            _dashboardRepository = dashboardRepository;
            _quizRepository = quizRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = _contextAccessor.HttpContext.User.GetUserId();
            var userQuizes = await _dashboardRepository.GetAllTeacherQuizzes();
            var dashboardViewModel = new DashboardViewModel()
            {
                Quizzes = userQuizes,
                UserId = currentUser
            };
            return View(dashboardViewModel);
        }







        [ActionName("Upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, DashboardViewModel dashboardViewModel)
        {
            // TODO: try and catch

            Quiz quiz = new Quiz();
            quiz.QuizName = file.FileName;
            quiz.DateCreated = DateTime.Now;
            quiz.UserId = dashboardViewModel.UserId;
            _quizRepository.Add(quiz);

            Section section = new Section();
            section.SectionName = "Default Question Pool";
            section.QuizId = quiz.Id;
            _quizRepository.Add(section);

            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                while (!fileReader.EndOfStream)
                {
                    Question question = new Question(); ;
                    List<Answer> answers = new List<Answer>();

                    var line = fileReader.ReadLine();
                    var values = line.Split("\t");

                    question.QuestionType = Enum.Parse<QuestionType>(values[0]);
                    question.QuestionTitle = values[1];
                    question.SectionId = section.Id;
                    _quizRepository.Add(question);

                    for (int i = 2; i < values.Length; i += 2)
                    {
                        Answer answer = new Answer();
                        answer.QuestionAnswer = values[i];
                        answer.isCorrect = values[i + 1];
                        answer.QuestionId = question.Id;
                        answers.Add(answer);
                    }

                    _quizRepository.Add(answers);

                }
            }
            return RedirectToAction("Index", "Dashboard");
        }






    }
}