using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quizard.Models;

namespace Quizard.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        // Pass to the base class which is "context"
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        // Which entities to map into the DB
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Answer> Answers { get; set; }
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite key
            builder.Entity<UserModule>()
                .HasKey(um => new { um.UserId, um.ModuleId });

            // Relation for user to have many modules
            builder.Entity<UserModule>()
                .HasOne(um => um.Module)
                .WithMany(um => um.Users)
                .HasForeignKey(um => um.ModuleId);

            // Relation for module to have many users
            builder.Entity<UserModule>()
                .HasOne(um => um.User)
                .WithMany(um => um.Modules)
                .HasForeignKey(um => um.UserId);
        }

    }
}
