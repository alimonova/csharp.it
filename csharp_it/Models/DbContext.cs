using csharp_it.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace csharp_it.Models
{
	public class DbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<PracticalExample> PracticalExamples { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tarif> Tarifs { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<UsefulResource> UsefulResources { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<TarifAccess> TarifAccesses { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbContext()
        { }

        public DbContext(DbContextOptions<DbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnswerConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());
            modelBuilder.ApplyConfiguration(new CourseConfiguration());
            modelBuilder.ApplyConfiguration(new LessonConfiguration());
            modelBuilder.ApplyConfiguration(new PracticalExampleConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new TarifConfiguration());
            modelBuilder.ApplyConfiguration(new UsefulResourceConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserCourseConfiguration());
            modelBuilder.ApplyConfiguration(new UserTaskConfiguration());
            modelBuilder.ApplyConfiguration(new AccessConfiguration());
            modelBuilder.ApplyConfiguration(new TarifAccessConfiguration());
            modelBuilder.ApplyConfiguration(new SolutionConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

