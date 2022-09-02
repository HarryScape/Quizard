using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class TakeQuizController : Controller
    {

        private readonly IQuizRepository _quizRepository;
        private readonly ITakeQuizRepository _takeQuizRepository;
        private readonly IQuizParserService _quizParserService;
        private readonly IHttpContextAccessor _contextAccessor;


        public TakeQuizController(IQuizRepository quizRepository, IQuizParserService quizParserService,
            IHttpContextAccessor contextAccessor, ITakeQuizRepository takeQuizRepository)
        {
            _quizRepository = quizRepository;
            _quizParserService = quizParserService;
            _contextAccessor = contextAccessor;
            _takeQuizRepository = takeQuizRepository;
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
            //var userQuizAttempt = new UserQuizAttempt()
            //{
            //    QuizId = quizId,
            //    UserId = currentUser,
            //    TimeStarted = DateTime.Now,
            //    IsMarked = false,
            //    ReleaseFeedback = false,
            //};
            //_takeQuizRepository.Add(userQuizAttempt);

            TakeQuizViewModel takeQuizViewModel = await _quizParserService.GenerateTakeQuizViewModel(quizId);
            //takeQuizViewModel.AttemptId = userQuizAttempt.Id;

            return PartialView("_TakeSectionPartial", takeQuizViewModel);
        }

        public async Task<IActionResult> NextSectionNavigation(int quizId, int index)
        {
            TakeQuizViewModel takeQuizViewModel = await _quizParserService.GenerateTakeQuizViewModel(quizId);
            List<Section> sections = (List<Section>)takeQuizViewModel.Sections;
            if (index < sections.Count - 1)
            {
                index++;
                takeQuizViewModel.Section = sections[index];
            }

            return PartialView("_TakeSectionPartial", takeQuizViewModel);
        }


        public async Task<IActionResult> PreviousSectionNavigation(int quizId, int index)
        {
            TakeQuizViewModel takeQuizViewModel = await _quizParserService.GenerateTakeQuizViewModel(quizId);
            List<Section> sections = (List<Section>)takeQuizViewModel.Sections;
            if (index > 0)
            {
                index--;
                takeQuizViewModel.Section = sections[index];
            }

            return PartialView("_TakeSectionPartial", takeQuizViewModel);
        }


        // public async Task<IActionResult> SubmitResponse(List<string> sectionResponse){
        // generate VM and load question responses.
        //}


        // public async Task<IActionResult> CompleteQuiz(int quizId)

    }
}
