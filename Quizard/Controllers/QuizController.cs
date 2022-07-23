﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;


using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Quizard.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        // UPLOAD

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Quiz> quizzes = await _quizRepository.GetAll();
            return View(quizzes);
        }



        // TODO:
        // Create a method for for controlling different upload types:
        // public async Task<IActionResult> Upload(IFormFile file) {
        //      _quizParserService.CheckDataType() { }
        //   if file.filetype = x then UploadTxt() else UploadXML() etc blackboard/other VLE. 


        //[ActionName("Upload")]
        //[HttpPost]
        //public async Task<IActionResult> Upload(IFormFile file, DashboardViewModel dashboardViewModel)
        //{
        //    // TODO: try and catch

        //    Quiz quiz = new Quiz();
        //    quiz.QuizName = file.FileName;
        //    quiz.DateCreated = DateTime.Now;
        //    quiz.UserId = dashboardViewModel.UserId;
        //    _quizRepository.Add(quiz);

        //    Section section = new Section();
        //    section.SectionName = "Default Question Pool";
        //    section.QuizId = quiz.Id;
        //    _quizRepository.Add(section);

        //    using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
        //    {
        //        while (!fileReader.EndOfStream)
        //        {
        //           Question question = new Question(); ;
        //            List<Answer> answers = new List<Answer>();

        //            var line = fileReader.ReadLine();
        //            var values = line.Split("\t");

        //            question.QuestionType = Enum.Parse<QuestionType>(values[0]);
        //            question.QuestionTitle = values[1];
        //            question.SectionId = section.Id;
        //            _quizRepository.Add(question);

        //            for (int i = 2; i < values.Length; i += 2)
        //            {
        //                Answer answer = new Answer();
        //                answer.QuestionAnswer = values[i];
        //                answer.isCorrect = values[i + 1];
        //                answer.QuestionId = question.Id;
        //                answers.Add(answer);
        //            }

        //            _quizRepository.Add(answers);

        //        }
        //    }
        //    return RedirectToAction("Index", "Dashboard");
        //}


        // Sructure Quiz Page
        [ActionName("Create")]
        public async Task<IActionResult> Create(int QuizId)
        {
            var quizViewModel = new CreateQuizViewModel();
            quizViewModel.Quiz = await _quizRepository.GetQuizById(QuizId);
            quizViewModel.Sections = await _quizRepository.GetQuizSections(QuizId);
            quizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(QuizId);
            quizViewModel.Answers = await _quizRepository.GetSpecificAnswers(QuizId);
            // todo: question type

            return View(quizViewModel);
        }

        [HttpPost]
        public ActionResult SectionPartialView(string sectionName, int quizId)
        {

            var section = new Section()
            {
                SectionName = sectionName,
                QuizId = quizId
            };
            _quizRepository.Add(section);

            CreateQuizViewModel quizVM = new CreateQuizViewModel();
            var p = PartialView("_Section", quizVM);
            return p;
        }


        // Adds Section straight to DB...
        [HttpPost]
        public async Task<IActionResult> AddSectionDB(string sectionName, int quizId)
        {
            var section = new Section()
            {
                SectionName = sectionName,
                QuizId = quizId
            };
            _quizRepository.Add(section);
            var quizViewModel = new CreateQuizViewModel();
            quizViewModel.Quiz = await _quizRepository.GetQuizById(quizId);
            quizViewModel.Sections = await _quizRepository.GetQuizSections(quizId);
            quizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(quizId);
            quizViewModel.Answers = await _quizRepository.GetSpecificAnswers(quizId);

            var p = PartialView("_Section", quizViewModel);
            return p;
        }


        [HttpPost]
        public async Task<IActionResult> SaveQuiz(List<QuestionJsonHelper> updates)
        {
            foreach(var item in updates)
            {
                Question question = await _quizRepository.GetQuestionById(Int32.Parse(item.Id));
                question.SectionId = Int32.Parse(item.SectionId);
                question.QuestionPosition = item.QuestionPosition;
                _quizRepository.Update(question);
            }

            var message = "State saved";
            return Json(message);
        }


    }
}
