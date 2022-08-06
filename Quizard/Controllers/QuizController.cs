using Microsoft.AspNetCore.Mvc;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuizParserService _quizParserService;

        public QuizController(IQuizRepository quizRepository, IQuizParserService quizParserService)
        {
            _quizRepository = quizRepository;
            _quizParserService = quizParserService;
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


        [ActionName("Create")]
        public async Task<IActionResult> Create(int quizId)
        {
            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

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

            CreateQuizViewModel quizViewModel = new CreateQuizViewModel();

            return PartialView("_Section", quizViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestionGroup(string groupName, int quizId, int sectionId)
        {
            var questionGroup = new Question()
            {
                QuestionType = QuestionType.GROUP,
                QuestionTitle = groupName,
                QuestionPosition = 0,
                SectionId = sectionId
            };
            _quizRepository.Add(questionGroup);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return PartialView("_Section", quizViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddSectionDB(string sectionName, int quizId)
        {
            var section = new Section()
            {
                SectionName = sectionName,
                QuizId = quizId
            };
            _quizRepository.Add(section);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return PartialView("_Section", quizViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> SaveQuiz(List<QuestionJsonHelper> updates)
        {
            foreach (var item in updates)
            {
                Question question = await _quizRepository.GetQuestionById(Int32.Parse(item.Id));
                question.SectionId = Int32.Parse(item.SectionId);
                question.QuestionPosition = (item.QuestionPosition + 1);
                question.ParentId = item.ParentId;
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

            return Json(new { redirectToUrl = Url.Action("Index", "Dashboard") });
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

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return PartialView("_Section", quizViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShowDeleteModal(int id)
        {
            Quiz quiz = await _quizRepository.GetQuizById(id);
            return PartialView("_DeleteModalPartial");
        }

        [HttpGet]
        public async Task<IActionResult> ShowEditModal(int id)
        {
            Question question = await _quizRepository.GetQuestionById(id);
            //Question question = new Question();
            return PartialView("_EditModalPartial", question);
        }

    }
}
