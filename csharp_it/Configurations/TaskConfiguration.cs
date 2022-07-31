using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = csharp_it.Models.Task;

namespace csharp_it.Configurations
{
	public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.ToTable("Tasks");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Text).IsRequired();
            builder.Property(t => t.Number).IsRequired();
            builder.Property(t => t.Example).IsRequired();

            builder
                .HasMany(t => t.UserTasks)
                .WithOne(u => u.Task)
                .HasForeignKey(u => u.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(t => t.Solutions)
                .WithOne(s => s.Task)
                .HasForeignKey(s => s.TaskId);
        }
    }
}

