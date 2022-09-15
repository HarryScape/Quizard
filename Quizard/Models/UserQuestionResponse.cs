using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class UserQuestionResponse
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        [ForeignKey("UserQuizAttempt")]
        public int UserQuizAttemptId { get; set; }
        public UserQuizAttempt UserQuizAttempt { get; set; }
        public int? AnswerId { get; set; }
        public Answer? Answer { get; set; }
        public string? AnswerResponse { get; set; }
        public string? AnswerFeedback { get; set; }
        public double? MarkAwarded { get; set; }
    }
}
