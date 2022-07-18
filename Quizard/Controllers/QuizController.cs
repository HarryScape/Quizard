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

            //quizVM.Sections.Add(section);
            //return PartialView("_Section", section);
            return Json(section);
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
            //return Json(section);
            var quizViewModel = new CreateQuizViewModel();
            quizViewModel.Quiz = await _quizRepository.GetQuizById(quizId);
            quizViewModel.Sections = await _quizRepository.GetQuizSections(quizId);
            quizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(quizId);
            quizViewModel.Answers = await _quizRepository.GetSpecificAnswers(quizId);

            var p = PartialView("_Section", quizViewModel);
            return p;
        }

        //public async Task<JsonResult> UpdateQuizPosition(Question questionArray)
        //{
        //    var message = "";
        //    if (questionArray == null)
        //    {
        //        message = "No changes made";
        //    }

        //    if (questionArray != null)
        //    {
        //        foreach (var questionItem in questionArray)
        //        {
        //            //Question q = await _quizRepository.GetQuestionById(question);
        //            // update position
        //            var question = new Question()
        //            {
        //                question.Id = questionItem.Id

        //            };
        //            //q.QuestionPosition = 11; //placeholder
        //            //q.SectionId = 99; // placeholder
        //            // _quizRepository.Update(q);
        //            message = "Question Position Updated";
        //        }
        //    }
        //    return Json(message);
        //}

        [HttpPost]
        public async Task<ActionResult> SaveQuiz(string questionArray)
        {
            int i = 0;
            List<QuestionJsonHelper> questionList = JsonConvert.DeserializeObject<List<QuestionJsonHelper>>(questionArray);

            foreach (var item in questionList)
            {
                Question question = await _quizRepository.GetQuestionById(item.Id);
                question.SectionId = item.SectionId;
                question.QuestionPosition = item.QuestionPosition;
                _quizRepository.Update(question);
            }

          

            //JArray array = JArray.Parse(questionArray);
            //foreach (JObject obj in array.Children<JObject>())
            //{
            //    foreach (JProperty singleProp in obj.Properties())
            //    {
            //        string name = singleProp.Name;
            //        string value = singleProp.Value.ToString();
            //        //Do something with name and value
            //        //System.Windows.MessageBox.Show("name is "+name+" and value is "+value);
            //    }
            //}

            //var testArray = JArray.Parse(questionArray).Children<JObject>()
            //         .SelectMany(x => x.Properties().Select(c => c.Value.Value<string>()));

            //var result = JsonConvert.SerializeObject(testArray.SelectMany(x => JArray.Parse(x)));


            var message = "I hate ajax";
            return Json(message);
        }


    }
}
