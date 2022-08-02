using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    [NotMapped]
    public class QuestionJsonHelper
    {
        public string Id { get; set; }
        public string SectionId { get; set; }
        public int QuestionPosition { get; set; }
        public int? ParentId { get; set; }
    }
}
