using Microsoft.EntityFrameworkCore;
using Quizard.Models;

namespace Quizard.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Pass to the base class which is "context"
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // Which entities to map into the DB
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
