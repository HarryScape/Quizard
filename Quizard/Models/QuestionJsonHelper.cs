using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    [NotMapped]
    public class QuestionJsonHelper
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("SectionId")]
        public int SectionId { get; set; }
        [JsonProperty("QuestionPosition")]
        public int QuestionPosition { get; set; }
    }
}
