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

            Quiz quiz = await _quizRepository.GetQuizById(updates[0].QuizId);
            quiz.DateCreated = DateTime.Now;
            _quizRepository.Update(quiz);

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
                foreach (Question item in questions)
                {
                    IEnumerable<Answer> answers = await _quizRepository.GetAnswersByQuestion(item.Id);
                    if (answers != null)
                    {
                        _quizRepository.DeleteAns(answers);
                    }
                }
                _quizRepository.DeleteQuestions(questions);
            }
            _quizRepository.Delete(section);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return PartialView("_Section", quizViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuestion(int questionId, int quizId)
        {
            Question question = await _quizRepository.GetQuestionById(questionId);
            var children = await _quizRepository.GetChildQuestions(questionId);

            if (children.Any())
            {
                foreach (Question item in question.Children)
                {
                    item.ParentId = question.ParentId;
                    _quizRepository.Update(item);
                }
            }

            IEnumerable<Answer> answers = await _quizRepository.GetAnswersByQuestion(questionId);
            if (answers != null)
            {
                _quizRepository.DeleteAns(answers);
            }
            _quizRepository.Delete(question);

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
            return PartialView("_EditModalPartial", question);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuestion(Question updatedQuestion)
        {
            Question question = await _quizRepository.GetQuestionById(updatedQuestion.Id);
            question.QuestionTitle = updatedQuestion.QuestionTitle;
            question.Mark = updatedQuestion.Mark;
            question.NegativeMark = updatedQuestion.NegativeMark;
            question.ErrorMargin = updatedQuestion.ErrorMargin;
            var section = await _quizRepository.GetSectionById(question.SectionId);

            _quizRepository.Update(question);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(section.QuizId);
            return PartialView("_Section", quizViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> ShowEditSectionModal(int id)
        {
            Section section = await _quizRepository.GetSectionById(id);
            return PartialView("_EditSectionModalPartial", section);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSection(Section updatedSection)
        {
            Section section = await _quizRepository.GetSectionById(updatedSection.Id);
            section.SectionName = updatedSection.SectionName;
            section.RequiredQuestions = updatedSection.RequiredQuestions;

            _quizRepository.Update(section);

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(section.QuizId);
            return PartialView("_Section", quizViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> ShowAddQuestionModal(int id, int sectionId)
        {
            var addQuestionViewModel = new AddQuestionViewModel();
            addQuestionViewModel.Quiz = await _quizRepository.GetQuizById(id);
            addQuestionViewModel.SectionId = sectionId;
            addQuestionViewModel.QuestionTypeList = await _quizParserService.GenerateQuestionTypes();
            return PartialView("_AddQuestionModalPartial", addQuestionViewModel);
        }

        [ActionName("AddQuestion")]
        [HttpPost]
        public async Task<IActionResult> AddQuestion(List<string> questionBody, List<string> answers, List<string> answersCheck)
        {
            Question question = new Question()
            {
                SectionId = Int32.Parse(questionBody[5]),
                QuestionTitle = questionBody[0],
                QuestionType = Enum.Parse<QuestionType>(questionBody[6]),
            };
            if (questionBody[1] != null)
            {
                question.Mark = Int32.Parse(questionBody[1]);
            }
            if (questionBody[2] != null)
            {
                question.ErrorMargin = double.Parse(questionBody[2]);
            }
            if (questionBody[3] != null)
            {
                question.NegativeMark = double.Parse(questionBody[3]);
            }

            _quizRepository.Add(question);

            for (int i = 0; i < answers.Count; i++)
            {
                Answer answer = new Answer()
                {
                    QuestionAnswer = answers[i],
                    QuestionId = question.Id,
                };
                if (answersCheck[i].Equals("true"))
                {
                    answer.isCorrect = true;
                }
                _quizRepository.Add(answer);
            }

            var quizViewModel = await _quizParserService.GenerateQuizViewModel(Int32.Parse(questionBody[4]));
            return PartialView("_Section", quizViewModel);
        }

    }
}
