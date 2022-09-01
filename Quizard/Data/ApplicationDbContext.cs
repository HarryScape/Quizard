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

        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserQuizAttempt> UserQuizAttempts { get; set; }
        public DbSet<UserQuestionResponse> UserQuestionResponses { get; set; }

        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // USER MODULES
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



            // USER QUIZ ATTEMPTS
            // Relation for user to have many quiz attempts
            builder.Entity<UserQuizAttempt>()
                .HasOne(qa => qa.Quiz)
                .WithMany(qa => qa.UserQuizAttempts)
                .HasForeignKey(qa => qa.UserId);

            // Relation for quiz to have many user attempts
            builder.Entity<UserQuizAttempt>()
                .HasOne(qa => qa.User)
                .WithMany(qa => qa.UserQuizAttempts)
                .HasForeignKey(qa => qa.QuizId);



            // responses
            // Relation for user attempt to have many question responses
            builder.Entity<UserQuestionResponse>()
                .HasOne(qr => qr.UserQuizAttempt)
                .WithMany(qr => qr.QuestionResponses)
                .HasForeignKey(qr => qr.QuestionId);

            // Relation for question to have many user responses
            builder.Entity<UserQuestionResponse>()
                .HasOne(qr => qr.Question)
                .WithMany(qr => qr.QuestionResponses)
                .HasForeignKey(qr => qr.UserQuizAttemptId);

        }



    }
}
