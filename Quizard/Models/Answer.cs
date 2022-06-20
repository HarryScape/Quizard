using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Answer
    {
        [Key]
        public string Id { get; set; }
        public bool? isCorrect { get; set; }

        [Required]
        public string QuestionAnswer { get; set; }
        [Required]
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }

    }
}
