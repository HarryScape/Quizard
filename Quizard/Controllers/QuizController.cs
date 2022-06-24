using Microsoft.AspNetCore.Mvc;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;


namespace Quizard.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // TODO:
        // Create a method for for controlling different upload types:
        // public async Task<IActionResult> Upload(IFormFile file) {
        //      _quizParserService.CheckDataType() { }
        //   if file.filetype = x then UploadTxt() else UploadXML() etc. 


        [ActionName("Upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // TODO: Put the whole thing in a try and catch

            Quiz quiz = new Quiz();
            quiz.QuizName = file.FileName;
            quiz.DateCreated = DateTime.Now;
            _quizRepository.Add(quiz);

            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                while (!fileReader.EndOfStream)
                {
                    Question question = new Question();;
                    List<Answer> answers = new List<Answer>();

                    var line = fileReader.ReadLine();
                    var values = line.Split("\t");

                    question.QuestionType = Enum.Parse<QuestionType>(values[0]);
                    question.QuestionTitle = values[1];
                    question.QuizId = quiz.Id;
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
            //return RedirectToAction("Upload");
        }





















    }
}
