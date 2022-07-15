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


        [ActionName("Upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // TODO: try and catch

            Quiz quiz = new Quiz();
            quiz.QuizName = file.FileName;
            quiz.DateCreated = DateTime.Now;
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
            return View();
        }






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


        public IActionResult SectionPartialView()
        {
            var QuizVM = new CreateQuizViewModel();

            return PartialView("_Section", QuizVM);
        }


        // Working with ajax call
        [HttpPost]
        public ActionResult AddSection(string sectionName, int quizId)
        {
            var quizVM = new CreateQuizViewModel();
            var section = new Section()
            {
                SectionName = sectionName,
                QuizId = quizId
            };

            //quizVM.Sections.

            return Json(section);
        }



    }
}
