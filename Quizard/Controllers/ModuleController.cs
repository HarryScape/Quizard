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
        public ModuleController(IHttpContextAccessor contextAccessor, IModuleRepository moduleRepository)
        {
            _contextAccessor = contextAccessor;
            _moduleRepository = moduleRepository;
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

        //public async Task<IActionResult> EditModule()

        //public async Task<IActionResult> DeleteModule()

        //public async Task<IActionResult> EnrollStudents()

        //public async Task<IActionResult> RemoveStudents()
    }
}
