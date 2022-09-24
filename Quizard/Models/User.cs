using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Quizard.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime DateRegistered { get; set; }

        public ICollection<Quiz>? UserQuizzes { get; set; }
        public ICollection<UserModule>? Modules { get; set; }
        public ICollection<UserQuizAttempt>? UserQuizAttempts { get; set; }

    }
}
