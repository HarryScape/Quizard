using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class UserQuizAttempt
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Quiz")]
        public int? QuizId { get; set; }
        public Quiz? Quiz { get; set; }

        public double? Score { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime? TimeCompleted { get; set; }
        public bool IsMarked { get; set; }
        public bool ReleaseFeedback { get; set; }
        public ICollection<UserQuestionResponse>? QuestionResponses { get; set; }

    }
}
