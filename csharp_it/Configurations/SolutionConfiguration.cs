using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class SolutionConfiguration : IEntityTypeConfiguration<Solution>
    {
        public void Configure(EntityTypeBuilder<Solution> builder)
        {
            builder.ToTable("Solutions");
            builder.HasKey(s => s.Id);

            builder
                .HasOne(s => s.User)
                .WithMany(u => u.Solutions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(s => s.Task)
                .WithMany(t => t.Solutions)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(s => s.TaskId);
        }
    }
}

