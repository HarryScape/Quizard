using Microsoft.AspNetCore.Mvc;
using Quizard.Models;

namespace Quizard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SectionsController : ControllerBase
    {
        [HttpPost]
        public ActionResult AddSection(string sectionName, int quizId)
        {
            var section = new Section()
            {
                SectionName = sectionName,
                QuizId = quizId
            };
            return Ok();
        }

        [HttpPost]
        public ActionResult Index()
        {
            return Ok();
        }
    }
}