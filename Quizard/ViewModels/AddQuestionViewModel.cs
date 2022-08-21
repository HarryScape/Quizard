using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.Models;

namespace Quizard.ViewModels
{
    public class AddQuestionViewModel
    {
        public Quiz Quiz { get; set; }
        public Question Question { get; set; }
        public int QuizId { get; set; }
        //public int SectionId { get; set; }
        //public IEnumerable<Answer> Answers { get; set; }
        //public IEnumerable<bool> CorrectAnswers { get; set; }
        public Answer Answer { get; set; }
        public bool Correct { get; set; }
        public IEnumerable<SelectListItem>? QuestionTypeList { get; set; }
    }
}
