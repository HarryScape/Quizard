using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace Quizard.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Required")]
        public string EmailAddress { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Name Required")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        //[Display(Name = "Are You A Student Or Teacher?")]
        public IEnumerable<SelectListItem>? RoleList { get; set; }
        [Required(ErrorMessage = "Choose account type")]
        [Display(Name = "Select Role")]
        public string RoleSelected { get; set; }
    }
}
