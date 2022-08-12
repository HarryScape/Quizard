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
        public string? Module { get; set; }

        // USER ID NEED TO BE CHANGED TO A STRING.... DONT FORGET!!!!
        // Quiz can only have one user. ? NULLABLE FOR NOW. 
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User? User { get; set; }
        // prop LMS?
        public int? TimeLimit { get; set; }
        public bool Shuffled { get; set; }
        public bool Deployed { get; set; }

        // Quiz can have many sections
        public ICollection<Section> QuizSections { get; set; }

    }
}
