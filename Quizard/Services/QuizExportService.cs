using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Services
{
    public class QuizExportService : IQuizExportService
    {
        private readonly IQuizRepository _quizRepository;

        public QuizExportService(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public async Task<ExportQuizViewModel> GenerateQuizViewModel(int id)
        {
            var quizViewModel = new ExportQuizViewModel();
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

        public void GenerateDocx(ExportQuizViewModel exportQuizViewModel)
        {
            int i = 5;
        }
    }
}
