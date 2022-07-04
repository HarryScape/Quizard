using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        // public bool? isCorrect { get; set; }
        public string? isCorrect { get; set; }
        public string? QuestionAnswer { get; set; }
        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        // Maybe AnswerOrder for randomisation
    }
}

