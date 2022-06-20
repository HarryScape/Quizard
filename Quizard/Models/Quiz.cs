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


        // Quiz can only have one user
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }


        // Quiz can have many questions
        public ICollection<Question> QuizQuestions { get; set; }

    }
}
