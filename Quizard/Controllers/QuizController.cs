using Microsoft.AspNetCore.Mvc;
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
            foreach (var item in updates)
            {
                Question question = await _quizRepository.GetQuestionById(Int32.Parse(item.Id));
                question.SectionId = Int32.Parse(item.SectionId);
                question.QuestionPosition = item.QuestionPosition;
                _quizRepository.Update(question);
            }

            var message = "State saved";
            return Json(message);
        }

        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _quizRepository.GetQuizById(id);
            IEnumerable<Answer> answers = await _quizRepository.GetSpecificAnswers(id);
            IEnumerable<Section> sections = await _quizRepository.GetQuizSections(id);
            IEnumerable<Question> questions = await _quizRepository.GetQuestionByQuizID(id);

            if (quiz == null) return View("Error");

            _quizRepository.DeleteAns(answers);
            _quizRepository.DeleteQuestions(questions);
            _quizRepository.DeleteSections(sections);
            _quizRepository.Delete(quiz);

            //return RedirectToAction("Index", "Dashboard");
            return Json(new { redirectToUrl = Url.Action("Index", "Dashboard") });
            return Json(Url.Action("Index", "Dashboard"));
        }



        [HttpPost]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            var section = await _quizRepository.GetSectionById(sectionId);
            int quizId = section.QuizId;

            IEnumerable<Question> questions = await _quizRepository.GetQuestionBySectionID(sectionId);
            if (questions != null)
            {
                _quizRepository.DeleteQuestions(questions);
            }
            _quizRepository.Delete(section);

            var quizViewModel = new CreateQuizViewModel();
            quizViewModel.Quiz = await _quizRepository.GetQuizById(quizId);
            quizViewModel.Sections = await _quizRepository.GetQuizSections(quizId);
            quizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(quizId);
            quizViewModel.Answers = await _quizRepository.GetSpecificAnswers(quizId);

            var p = PartialView("_Section", quizViewModel);
            return p;
        }
    }
}
