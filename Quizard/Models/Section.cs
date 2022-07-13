using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public string SectionName { get; set; }
        public int SectionOrder { get; set; }
        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }


        // Section can have many questions
        public ICollection<Question> QuizQuestions { get; set; }
    }
}
