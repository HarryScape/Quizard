using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        public ModuleController(IHttpContextAccessor contextAccessor, IModuleRepository moduleRepository, 
            IQuizRepository quizRepository, IDashboardRepository dashboardRepository, UserManager<User> userManager)
        {
            _contextAccessor = contextAccessor;
            _moduleRepository = moduleRepository;
            _quizRepository = quizRepository;
            _dashboardRepository = dashboardRepository;
            _userManager = userManager;
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

        // Generate Modal popup
        [HttpGet]
        public async Task<IActionResult> ShowAddModuleModal()
        {
            Module module = new Module();
            return PartialView("_AddModuleModalPartial", module);
        }


        /// <summary>
        /// Recieves form data and creates a new module
        /// </summary>
        /// <param name="newModule"></param>
        [HttpPost]
        public async Task<IActionResult> AddModule(Module newModule)
        {
            if(newModule.ModuleCode != null)
            {
                Module module = new Module()
                {
                    Id = newModule.Id,
                    Description = newModule.Description,
                    ModuleCode = newModule.ModuleCode,
                    ModuleLeaderId = _contextAccessor.HttpContext.User.GetUserId(),
                };
                _moduleRepository.Add(module);
            }
           
            return RedirectToAction("Index", "Module");
        }

        // Generate Modal popup
        [HttpGet]
        public async Task<IActionResult> ShowEditModuleModal(int id)
        {
            Module module = await _moduleRepository.GetModuleById(id);
            return PartialView("_EditModuleModalPartial", module);
        }


        /// <summary>
        /// Edits an existing module
        /// </summary>
        /// <param name="updatedModule"></param>
        [ActionName("EditModule")]
        [HttpPost]
        public async Task<IActionResult> EditModule(Module updatedModule)
        {
            if(updatedModule.ModuleCode != null)
            {
                Module module = await _moduleRepository.GetModuleById(updatedModule.Id);
                module.Description = updatedModule.Description;
                module.ModuleCode = updatedModule.ModuleCode;
                _moduleRepository.Update(module);
            }

            return RedirectToAction("Index", "Module");
        }


        public async Task<IActionResult> DeleteModule(int id)
        {
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


        // Generate Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowEnrollModuleModal(int id)
        {
            Module module = await _moduleRepository.GetModuleById(id);
            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel();
            enrollmentViewModel.Module = module;
            IEnumerable<User> students = await _moduleRepository.GetStudentsByModule(module.Id);

            if (students.Any())
            {
                enrollmentViewModel.Students = students.ToList();
            }

            return PartialView("_EnrollStudentsModalPartial", enrollmentViewModel);
        }


        /// <summary>
        /// Enrolls a list of students into a module so they can take deployed quizzes belonging to the module
        /// </summary>
        /// <param name="studentForm"></param>
        /// <param name="moduleId"></param>
        [ActionName("EnrollStudents")]
        [HttpPost]
        public async Task<IActionResult> EnrollStudents(string studentForm, int moduleId)
        {
            // harry@student.edu, leo@student.edu, bob@bob.com, cat@cat.cat, random@goblin.com, student@student.com
            Module module = await _moduleRepository.GetModuleById(moduleId);
            List<string> studentList = studentForm.Split(',').ToList<string>();

            for (int i = 0; i < studentList.Count; i++)
            {
                studentList[i] = string.Concat(studentList[i].Where(c => !char.IsWhiteSpace(c)));
            }

            foreach(var studentEmail in studentList)
            {
                var user = await _userManager.FindByEmailAsync(studentEmail);
                if (user != null)
                {
                    UserModule userModule = new UserModule()
                    {
                        UserId = user.Id,
                        ModuleId = moduleId,
                    };                   
                    _moduleRepository.Add(userModule);
                }
            }
            return RedirectToAction("Index", "Module");
        }

        // Generate Modal Popup
        [HttpGet]
        public async Task<IActionResult> ShowRemoveStudentModal(int id)
        {
            Module module = await _moduleRepository.GetModuleById(id);
            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel();
            enrollmentViewModel.Module = module;
            IEnumerable<User> students = await _moduleRepository.GetStudentsByModule(module.Id);

            if (students.Any())
            {
                enrollmentViewModel.Students = students.ToList();
            }

            return PartialView("_RemoveStudentsModalPartial", enrollmentViewModel);
        }


        /// <summary>
        /// Removes a student from a module, preventing them seeing module quizzes in their dashboard
        /// </summary>
        /// <param name="studentEmail"></param>
        /// <param name="moduleId"></param>
        [ActionName("RemoveStudents")]
        [HttpPost]
        public async Task<IActionResult> RemoveStudents(string studentEmail, int moduleId)
        {
            var user = await _userManager.FindByEmailAsync(studentEmail);
            if (user != null)
            {
                UserModule userModule = await _moduleRepository.GetSpecificUserModule(user.Id, moduleId);
                int r = 4;
                _moduleRepository.Delete(userModule);
            }

            Module module = await _moduleRepository.GetModuleById(moduleId);
            EnrollmentViewModel enrollmentViewModel = new EnrollmentViewModel();
            enrollmentViewModel.Module = module;
            IEnumerable<User> students = await _moduleRepository.GetStudentsByModule(module.Id);

            if (students.Any())
            {
                enrollmentViewModel.Students = students.ToList();
            }

            return PartialView("_RemoveStudentsModalPartial", enrollmentViewModel);
        }

    }
}




