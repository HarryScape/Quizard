using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public string QuizName { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("Module")]
        public int? ModuleId { get; set; }
        public Module? Module { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int? TimeLimit { get; set; }
        public int? ExtraTime { get; set; }
        public bool Shuffled { get; set; }
        public bool Deployed { get; set; }
        public ICollection<Section> QuizSections { get; set; }
        public ICollection<UserQuizAttempt>? UserQuizAttempts { get; set; }

    }
}
