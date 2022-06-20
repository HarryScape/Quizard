using System.ComponentModel.DataAnnotations;

namespace Quizard.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime DateRegistered { get; set; }

        public ICollection<Quiz> UserQuizzes { get; set; }
    }
}
