using System;
using csharp.it.Models; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp.it.Configurations
{
	public class UserTaskConfiguration : IEntityTypeConfiguration<UserTask>
    {
        public void Configure(EntityTypeBuilder<UserTask> builder)
        {
            builder.ToTable("UserTasks");
            builder.HasKey(ut => ut.Id);

            builder.Property(ut => ut.Status).IsRequired();

            builder
                .HasOne(ut => ut.Task)
                .WithMany(t => t.UserTasks)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(ut => ut.Student)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(ut => ut.StudentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

