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


        public async Task<IActionResult> SubmitResponse(List<string> questionTextIdList, List<string> textResponseList,
            List<string> questionCheckboxIdList, List<string> ansText, List<string> ansCorrect, int attemptId)
        {
            // generate VM and load question responses.
            // get response from db via attempt id and question id
            // if null create new response, if not null update existing response.

            // text
            for (int i = 0; i < textResponseList.Count; i++)
            {
                UserQuestionResponse response = new UserQuestionResponse()
                {
                    QuestionId = Convert.ToInt32(questionTextIdList[i]),
                    UserQuizAttemptId = Convert.ToInt32(attemptId),
                    AnswerResponse = textResponseList[i],
                };
                _takeQuizRepository.Add(response);
            }

            // checkbox
            for (int i = 0; i < ansCorrect.Count; i++)
            {
                if (ansCorrect[i].Equals("true"))
                {
                    UserQuestionResponse response = new UserQuestionResponse()
                    {
                        QuestionId = Convert.ToInt32(questionCheckboxIdList[i]),
                        UserQuizAttemptId = Convert.ToInt32(attemptId),
                        AnswerResponse = ansText[i],
                    };
                    _takeQuizRepository.Add(response);
                }
            }


            return null;
        }


        // public async Task<IActionResult> CompleteQuiz(int quizId)

    }
}
