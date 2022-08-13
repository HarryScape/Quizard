using Microsoft.AspNetCore.Mvc.Rendering;
using Quizard.Models;
using System.ComponentModel.DataAnnotations;

namespace Quizard.ViewModels
{
    public class EditQuizViewModel
    {
        public Quiz Quiz { get; set; }
        public IEnumerable<SelectListItem>? ModuleList { get; set; }
        public string ModuleSelected { get; set; }
    }
}
