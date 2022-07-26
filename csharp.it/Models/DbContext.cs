﻿using csharp.it.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace csharp.it.Models
{
	public class DbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<PracticalExample> PracticalExamples { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Tarif> Tarifs { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<UsefulResource> UsefulResources { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

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
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new LessonConfiguration());
            modelBuilder.ApplyConfiguration(new PracticalExampleConfiguration());
            modelBuilder.ApplyConfiguration(new QuestionConfiguration());
            modelBuilder.ApplyConfiguration(new TarifConfiguration());
            modelBuilder.ApplyConfiguration(new UsefulResourceConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new UserTaskConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

