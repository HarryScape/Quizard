namespace Quizard.Models
{
    public class UserModule
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
    }
}

