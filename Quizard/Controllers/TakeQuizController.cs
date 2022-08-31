using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;

namespace Quizard.Controllers
{
    public class TakeQuizController : Controller
    {

        private readonly IQuizRepository _quizRepository;
        private readonly IQuizParserService _quizParserService;

        public TakeQuizController(IQuizRepository quizRepository, IQuizParserService quizParserService)
        {
            _quizRepository = quizRepository;
            _quizParserService = quizParserService;

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
    }
}
