using Microsoft.AspNetCore.Mvc;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Components
{
    public class QuestionViewComponent : ViewComponent
    {
        //private readonly 
        //public QuestionViewComponent()
        //{

        //}

        public async Task<IViewComponentResult> InvokeAsync(IEnumerable<UserQuestionResponse> questionResponses, IEnumerable<Question> questions)
        {
            
            TakeQuestionViewModel takeQuestionViewModel = new TakeQuestionViewModel{
                QuestionResponses = questionResponses,
                Questions = questions,
            };
            return View(takeQuestionViewModel);
        }
    }
}
