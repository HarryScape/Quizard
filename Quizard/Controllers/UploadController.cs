using Microsoft.AspNetCore.Mvc;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;


namespace Quizard.Controllers
{
    public class UploadController : Controller
    {
        private readonly IQuizRepository _quizRepository;

        public UploadController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }




        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }





        [ActionName("Upload")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
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
                    question.QuizId = 1; // remove this later just for testing.

                    for (int i = 2; i < values.Length; i += 2)
                    {
                        Answer answer = new Answer();
                        answer.QuestionAnswer = values[i];
                        answer.isCorrect = values[i + 1];
                        answers.Add(answer);
                    }

                    _quizRepository.Add(question);

                }
            }
            return View();
        }





















    }
}
