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
        [ForeignKey(nameof(ModuleLeader))]
        public string? UserId { get; set; }
        public User? ModuleLeader { get; set; }
        //[ForeignKey("Quiz")]
        //public int? QuizId { get; set; }
        //public Quiz? Quiz { get; set; }
        public ICollection<Quiz> ModuleQuizzes { get; set; }
    }
}
