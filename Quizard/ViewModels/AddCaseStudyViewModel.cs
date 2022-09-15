using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quizard.ViewModels
{
    public class AddCaseStudyViewModel
    {
        public string CaseStudyName { get; set; }
        public int QuizId { get; set; }
        public IEnumerable<SelectListItem>? SectionList { get; set; }
        [Required(ErrorMessage = "Choose section")]
        [Display(Name = "* Select Section")]
        public string SectionSelected { get; set; }
    }
}
