using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.Data;
using Quizard.Data.Enum;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.Repository;
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
        private readonly ICanvasParserService _canvasParserService;
        private readonly IMoodleParserService _moodleParserService;

        public DashboardController(IDashboardRepository dashboardRepository, IQuizRepository quizRepository,
            IHttpContextAccessor contextAccessor, IQuizParserService quizParserService, IBlackboardParserService blackboardParserService,
            ICanvasParserService canvasParserService, IMoodleParserService moodleParserService, IModuleRepository moduleRepository, 
            IQuizExportService quizExportService)
        {
            _dashboardRepository = dashboardRepository;
            _quizRepository = quizRepository;
            _contextAccessor = contextAccessor;
            _quizParserService = quizParserService;
            _blackboardParserService = blackboardParserService;
            _canvasParserService = canvasParserService;
            _moodleParserService = moodleParserService;
            _moduleRepository = moduleRepository;
            _quizExportService = quizExportService;
        }

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

        [HttpPost]
        public async Task<IActionResult> UploadQuiz(IFormFile file, DashboardViewModel dashboardViewModel)
        {
            string lms = await _quizParserService.GetQuizLMS(file);

            if (lms.Equals("Blackboard"))
            {
                await _blackboardParserService.ParseQuiz(file, dashboardViewModel);
            }
            else if (lms.Equals("Canvas"))
            {
                await _canvasParserService.ParseQuiz(file, dashboardViewModel);
            }
            else if (lms.Equals("Moodle"))
            {
                await _moodleParserService.ParseQuiz(file, dashboardViewModel);
            }
            else
            {
                return View("Error");
            }
            
            return RedirectToAction("Index", "Dashboard");
        }




        [HttpGet]
        public async Task<IActionResult> ShowOptionsModal(int id)
        {
            Quiz quiz = await _quizRepository.GetQuizById(id);

            //new stuff
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
            //return PartialView("_QuizOptionsPartial", quiz);
        }


        [ActionName("UpdateQuiz")]
        [HttpPost]
        public async Task<IActionResult> UpdateQuiz(Quiz updatedQuiz)
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

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> QuizDeployment(int quizId)
        {
            Quiz quiz = await _quizRepository.GetQuizById(quizId);
            quiz.Deployed = !quiz.Deployed;
            _quizRepository.Update(quiz);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> ExportQuiz(int quizId)
        {
            var exportQuizViewModel = await _quizExportService.GenerateQuizViewModel(quizId);
            _quizExportService.GenerateDocx(exportQuizViewModel);
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
