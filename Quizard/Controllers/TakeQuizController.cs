using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.Models;

namespace Quizard.Controllers
{
    public class TakeQuizController : Controller
    {

        private readonly IQuizRepository _quizRepository;
        private readonly IQuizParserService _quizParserService;
        private readonly IHttpContextAccessor _contextAccessor;


        public TakeQuizController(IQuizRepository quizRepository, IQuizParserService quizParserService, IHttpContextAccessor contextAccessor)
        {
            _quizRepository = quizRepository;
            _quizParserService = quizParserService;
            _contextAccessor = contextAccessor;
        }


        public async Task<IActionResult> Index(int quizId)
        {
            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return View(quizViewModel);
        }

        public async Task<IActionResult> Completed(int quizId)
        {
            var quizViewModel = await _quizParserService.GenerateQuizViewModel(quizId);

            return View(quizViewModel);
        }



        public async Task<IActionResult> BeginQuiz(int quizId)
        {
            var currentUser = _contextAccessor.HttpContext.User.GetUserId();
            var userQuizAttempt = new UserQuizAttempt()
            {
                QuizId = quizId,
                UserId = currentUser,
                TimeStarted = DateTime.Now,
                IsMarked = false,
                ReleaseFeedback = false,
            };

            // return section partial view...
            return null;
        }


        // public async Task<IActionResult> SubmitResponse(List<string> sectionResponse)


        // public async Task<IActionResult> CompleteQuiz(int quizId)

    }
}
