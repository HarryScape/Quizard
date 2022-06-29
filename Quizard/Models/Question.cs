using Quizard.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionTitle { get; set; }
        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<Answer> QuestionAnswers { get; set; }

        // TODO: add QuestionOrder attribute

    }
}
