using System.IO;
using System.Globalization;
using System.Linq;
using Quizard.Models;
using Quizard.Interfaces;
using Quizard.Data.Enum;
using Quizard.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Quizard.Services
{
    public class QuizParserService : IQuizParserService
    {

        private readonly IQuizRepository _quizRepository;

        public QuizParserService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<string> GetQuizType(IFormFile file)
        {
            string type = "";

            using (StreamReader fileReader = new StreamReader(file.OpenReadStream()))
            {
                string line = fileReader.ReadLine();

                if (char.IsDigit(line[0]))
                {
                    type = "MarkdownA";
                }
                else if (line.StartsWith("::"))
                {
                    type = "MarkdownB";
                }
                else if (char.IsUpper(line[0]) && char.IsUpper(line[1])){
                    type = "Blackboard";
                }
            }
            return type;
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

        public async Task<List<SelectListItem>> GenerateQuestionTypes()
        {
            List<SelectListItem> questionTypes = new List<SelectListItem>();

            questionTypes.Add(new SelectListItem()
            {
                Value = "MC",
                Text = "Multiple Choice"
            });
            questionTypes.Add(new SelectListItem()
            {
                Value = "MA",
                Text = "Multiple Answer"
            });
            questionTypes.Add(new SelectListItem()
            {
                Value = "ESS",
                Text = "Essay"
            });
            questionTypes.Add(new SelectListItem()
            {
                Value = "TF",
                Text = "True or False"
            });
            questionTypes.Add(new SelectListItem()
            {
                Value = "FIB",
                Text = "Fill in the Blank"
            });


            return (questionTypes);
        }


        public async Task<TakeQuizViewModel> GenerateTakeQuizViewModel(int id)
        {
            TakeQuizViewModel takeQuizViewModel = new TakeQuizViewModel();
            takeQuizViewModel.Quiz = await _quizRepository.GetQuizById(id);
            takeQuizViewModel.Sections = await _quizRepository.GetQuizSections(id);
            takeQuizViewModel.Questions = await _quizRepository.GetQuestionByQuizID(id);
            takeQuizViewModel.ParentQuestions = await _quizRepository.GetParentQuestions(id);
            takeQuizViewModel.Answers = await _quizRepository.GetSpecificAnswers(id);

            foreach (var question in takeQuizViewModel.ParentQuestions)
            {
                question.Children = (ICollection<Question>)await _quizRepository.GetChildQuestions(question.Id);
                question.QuestionAnswers = (ICollection<Answer>)await _quizRepository.GetAnswersByQuestion(question.Id);
            }
            foreach (var question in takeQuizViewModel.Questions)
            {
                question.Children = (ICollection<Question>)await _quizRepository.GetChildQuestions(question.Id);
                question.QuestionAnswers = (ICollection<Answer>)await _quizRepository.GetAnswersByQuestion(question.Id);
            }
            takeQuizViewModel.Section = takeQuizViewModel.Sections.First();

            return takeQuizViewModel;
        }

    }
}
