using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizard.Models
{
    [NotMapped]
    //[JsonObject]
    public class QuestionJsonHelper
    {
        //[JsonProperty("Id")]
        //[JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }
        //[JsonProperty("SectionId")]
        //[JsonProperty(PropertyName = "SectionId")]
        public string SectionId { get; set; }
        //[JsonProperty("QuestionPosition")]
        //[JsonProperty(PropertyName = "QuestionPosition")]
        public int QuestionPosition { get; set; }
    }
}
