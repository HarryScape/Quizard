using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.Models;
using Quizard.ViewModels;

namespace Quizard.Controllers
{
    public class ModuleController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IModuleRepository _moduleRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IDashboardRepository _dashboardRepository;
        public ModuleController(IHttpContextAccessor contextAccessor, IModuleRepository moduleRepository, 
            IQuizRepository quizRepository, IDashboardRepository dashboardRepository)
        {
            _contextAccessor = contextAccessor;
            _moduleRepository = moduleRepository;
            _quizRepository = quizRepository;
            _dashboardRepository = dashboardRepository;
        }


        public async Task<IActionResult> Index()
        {
            var currentUser = _contextAccessor.HttpContext.User.GetUserId();
            var userModules = await _moduleRepository.GetUserModules();

            var moduleViewModel = new ModuleViewModel()
            {
                Modules = userModules,
                UserId = currentUser
            };
            return View(moduleViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShowAddModuleModal()
        {
            Module module = new Module();
            return PartialView("_AddModuleModalPartial", module);
        }

        [HttpPost]
        public async Task<IActionResult> AddModule(Module newModule)
        {
            Module module = new Module()
            {
                Id = newModule.Id,
                Description = newModule.Description,
                ModuleCode = newModule.ModuleCode,
                UserId = _contextAccessor.HttpContext.User.GetUserId()
        };
            _moduleRepository.Add(module);

            return RedirectToAction("Index", "Module");
        }


        [HttpGet]
        public async Task<IActionResult> ShowEditModuleModal(int id)
        {
            Module module = await _moduleRepository.GetModuleById(id);
            return PartialView("_EditModuleModalPartial", module);
        }

        [ActionName("EditModule")]
        [HttpPost]
        public async Task<IActionResult> EditModule(Module updatedModule)
        {
            Module module = await _moduleRepository.GetModuleById(updatedModule.Id);
            module.Description = updatedModule.Description;
            module.ModuleCode = updatedModule.ModuleCode;
            _moduleRepository.Update(module);

            return RedirectToAction("Index", "Module");
        }

        public async Task<IActionResult> DeleteModule(int id)
        {
            // REMEMBER TO UNENROLL STUDENTS,

            Module module = await _moduleRepository.GetModuleById(id);
            var userQuizes = await _dashboardRepository.GetAllTeacherQuizzes();

            foreach (var userQuiz in userQuizes.Where(i => i.ModuleId == module.Id))
            {
                userQuiz.ModuleId = null;
                _quizRepository.Update(userQuiz);
            }
            _moduleRepository.Delete(module);

            return RedirectToAction("Index", "Module");
        }

        //public async Task<IActionResult> EnrollStudents()
        // input csv

        //public async Task<IActionResult> RemoveStudents()
        // table to remove
    }
}
