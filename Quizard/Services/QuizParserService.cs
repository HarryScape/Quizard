using System.IO;
using System.Globalization;
using System.Linq;
using Quizard.Models;
using Quizard.Interfaces;
using Quizard.Data.Enum;
using Quizard.ViewModels;

namespace Quizard.Services
{
    public class QuizParserService : IQuizParserService
    {

        private readonly IQuizRepository _quizRepository;

        public QuizParserService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }


        // need isValidQustion bools. 
        // public async Task<bool> isValidUpload()
        // public async Task<bool> isValidQuiz()
        // .txt, .csv, xml,.zip. answers[] <= 100, must have correct questiontype.

        public async Task<string> GetQuizLMS(IFormFile file)
        {
            string lms = "";

            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                string line = fileReader.ReadLine();

                if (char.IsDigit(line[0]))
                {
                    lms = "Canvas";
                }
                else if (line.StartsWith("::"))
                {
                    lms = "Moodle";
                }
                else if (char.IsUpper(line[0]) && char.IsUpper(line[1])){
                    lms = "Blackboard";
                }
            }
            return lms;
        }

        public async Task<CreateQuizViewModel> GenerateQuizViewModel(int id)
        {
            var quizViewModel = new CreateQuizViewModel();
            quizViewModel.Quiz = await _quizRepository.GetQuizById(id);
            quizViewModel.Sections = await _quizRepository.GetQuizSections(id);
            quizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(id);
            quizViewModel.ParentQuestions = await _quizRepository.GetParentQuestions(id);
            quizViewModel.Answers = await _quizRepository.GetSpecificAnswers(id);

            foreach (var question in quizViewModel.ParentQuestions)
            {
                question.Children = (ICollection<Question>)await _quizRepository.GetChildQuestions(question.Id);
                question.QuestionAnswers = (ICollection<Answer>)await _quizRepository.GetAnswersByQuestion(question.Id);
            }
            foreach (var question in quizViewModel.Questions)
            {
                question.Children = (ICollection<Question>)await _quizRepository.GetChildQuestions(question.Id);
                question.QuestionAnswers = (ICollection<Answer>)await _quizRepository.GetAnswersByQuestion(question.Id);
            }

            return quizViewModel;
        }


    }
}
