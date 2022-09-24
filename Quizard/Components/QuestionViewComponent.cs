using Microsoft.AspNetCore.Mvc;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Components
{
    public class QuestionViewComponent : ViewComponent
    {
        /// <summary>
        /// Builds a question view component 
        /// </summary>
        /// <param name="questionResponses"></param>
        /// <param name="questions"></param>
        /// <returns>ViewModel used by the component</returns>
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
