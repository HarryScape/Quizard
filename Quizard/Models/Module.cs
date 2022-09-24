using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        public string ModuleCode { get; set; }
        public string? Description { get; set; }
        public ICollection<Quiz> ModuleQuizzes { get; set; }
        public ICollection<UserModule> Users { get; set; }
        public string? ModuleLeaderId { get; set; }
    }
}
