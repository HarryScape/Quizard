using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Answer
    {
        [Key]
        public string Id { get; set; }
        // public bool? isCorrect { get; set; }
        public string? isCorrect { get; set; }
        // Add order attribute?
        public string? QuestionAnswer { get; set; }
        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

    }
}
