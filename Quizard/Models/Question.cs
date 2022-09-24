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
        public int? QuestionPosition { get; set; }
        [ForeignKey("Section")]
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public ICollection<Answer>? QuestionAnswers { get; set; }
        [ForeignKey(nameof(ParentQuestion))]
        public int? ParentId { get; set; }
        public Question? ParentQuestion { get; set; }
        public ICollection<Question>? Children { get; set; }
        public int? Mark { get; set; }
        public double? NegativeMark { get; set; }
        public double? ErrorMargin { get; set; }
        public ICollection<UserQuestionResponse>? QuestionResponses { get; set; }

    }
}
