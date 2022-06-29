using Microsoft.AspNetCore.Mvc;
using Quizard.Interfaces;
using Quizard.ViewModels;


namespace Quizard.Controllers
{
    public class CreeateController : Controller
    {

        private readonly IQuizRepository _quizRepository;

        public CreeateController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }


        public IActionResult Index()
        {
            return View();
        }

        //public async Task<IActionResult> Create(int QuizId)
        //{

        //}


        //public ViewResult Create()
        //{
        //    CreateQuizViewModel createQuizViewModel = new CreateQuizViewModel()
        //    {
        //        QuizVM = _quizRepository.GetAll(),
        //        QuestionVM = _quizRepository.GetAllQuestions(1),
        //        AnswerVM = _quizRepository.GetAllAns(1)
        //    };
        //    return View(createQuizViewModel);
        //}

    }
}
