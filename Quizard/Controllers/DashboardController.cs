using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IQuizExportService _quizExportService;
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IQuizParserService _quizParserService;
        private readonly IBlackboardParserService _blackboardParserService;
        private readonly IMarkdownParserService _markdownParserService;

        public DashboardController(IDashboardRepository dashboardRepository, IQuizRepository quizRepository,
            IHttpContextAccessor contextAccessor, IQuizParserService quizParserService, IBlackboardParserService blackboardParserService,
            IMarkdownParserService canvasParserService, IModuleRepository moduleRepository, 
            IQuizExportService quizExportService)
        {
            _dashboardRepository = dashboardRepository;
            _quizRepository = quizRepository;
            _contextAccessor = contextAccessor;
            _quizParserService = quizParserService;
            _blackboardParserService = blackboardParserService;
            _markdownParserService = canvasParserService;
            _moduleRepository = moduleRepository;
            _quizExportService = quizExportService;
        }


        /// <summary>
        /// Fetches all quizzes belonging to the current logged in user
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var currentUser = _contextAccessor.HttpContext.User.GetUserId();

            var userQuizes = new List<Quiz>();
            if (User.IsInRole("teacher"))
            {
                userQuizes = await _dashboardRepository.GetAllTeacherQuizzes();
            }
            else if (User.IsInRole("student"))
            {
                userQuizes = await _dashboardRepository.GetAllStudentQuizzes();
            }

                var dashboardViewModel = new DashboardViewModel()
            {
                Quizzes = userQuizes,
                UserId = currentUser,
            };

            foreach(var item in dashboardViewModel.Quizzes)
            {
                if(item.ModuleId != null)
                {
                    int modId = Convert.ToInt32(item.ModuleId);
                    item.Module = await _moduleRepository.GetModuleById(modId);
                }
            }
            return View(dashboardViewModel);
        }


        /// <summary>
        /// Identifies the quiz type and sends it to a parser method.
        /// </summary>
        /// <param name="file"> A .txt quiz </param>
        [HttpPost]
        public async Task<IActionResult> UploadQuiz(IFormFile file)
        {
            if(file == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            string type = await _quizParserService.GetQuizType(file);

            if (type.Equals("Blackboard"))
            {
                await _blackboardParserService.ParseQuiz(file);
            }
            else if (type.Equals("MarkdownA"))
            {
                await _markdownParserService.ParseQuizA(file);
            }
            else if (type.Equals("MarkdownB"))
            {
                await _markdownParserService.ParseQuizB(file);
            }
            else
            {
                //return View("Error");
                return RedirectToAction("Index", "Dashboard");
            }
            
            return RedirectToAction("Index", "Dashboard");
        }


        // Generates Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowOptionsModal(int id)
        {
            Quiz quiz = await _quizRepository.GetQuizById(id);

            IEnumerable<Module> userModules = await _moduleRepository.GetUserModules();
            List<SelectListItem> listItems = new List<SelectListItem>();

            foreach (var module in userModules)
            {
                string moduleId = module.Id.ToString();
                listItems.Add(new SelectListItem()
                {
                    Value = moduleId,
                    Text = module.ModuleCode
                });
            }
            EditQuizViewModel editQuizViewModel = new EditQuizViewModel()
            {
                Quiz = quiz,
                ModuleList = listItems,
                Deployed = quiz.Deployed,
                Shuffled = quiz.Shuffled
            };

            return PartialView("_QuizOptionsPartial", editQuizViewModel);
        }


        /// <summary>
        /// Updates the values of a quiz
        /// </summary>
        /// <param name="updatedQuiz"></param>
        [ActionName("UpdateQuiz")]
        [HttpPost]
        public async Task<IActionResult> UpdateQuiz(Quiz updatedQuiz)
        {
            if(updatedQuiz.QuizName != null)
            {
                int modId = Convert.ToInt32(updatedQuiz.ModuleId);
                Quiz quiz = await _quizRepository.GetQuizById(updatedQuiz.Id);
                quiz.QuizName = updatedQuiz.QuizName;
                quiz.TimeLimit = updatedQuiz.TimeLimit;
                quiz.Shuffled = updatedQuiz.Shuffled;
                quiz.Deployed = updatedQuiz.Deployed;
                quiz.ModuleId = updatedQuiz.ModuleId;
                quiz.Module = await _moduleRepository.GetModuleById(modId);
                quiz.DateCreated = DateTime.Now;
                _quizRepository.Update(quiz);
            }

            return RedirectToAction("Index", "Dashboard");
        }


        /// <summary>
        /// Toggle deploying a quiz so that a student can view and take the quiz from a their dashboard
        /// </summary>
        /// <param name="quizId"></param>
        public async Task<IActionResult> QuizDeployment(int quizId)
        {
            Quiz quiz = await _quizRepository.GetQuizById(quizId);
            quiz.Deployed = !quiz.Deployed;
            _quizRepository.Update(quiz);

            return RedirectToAction("Index", "Dashboard");
        }



        /// <summary>
        /// Generates a .docx formatted quiz for use in an API or to download
        /// </summary>
        /// <param name="quizId"></param>
        /// <returns>returns a .docx download of the quiz</returns>
        public async Task<IActionResult> ExportQuiz(int quizId)
        {
            var exportQuizViewModel = await _quizExportService.GenerateQuizViewModel(quizId);
            byte[] docToSend = await _quizExportService.GenerateDocx(exportQuizViewModel);

            // TODO: Buy GETMARKED membership to use the API further
            //string downloadUrl = await _quizExportService.GenerateQTI(docToSend);
            //return Redirect(downloadUrl);

            return File(docToSend, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"{exportQuizViewModel.Quiz.QuizName}.docx");
        }
    }
}
