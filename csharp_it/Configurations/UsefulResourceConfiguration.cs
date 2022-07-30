using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class UsefulResourceConfiguration : IEntityTypeConfiguration<UsefulResource>
    {
        public void Configure(EntityTypeBuilder<UsefulResource> builder)
        {
            builder.ToTable("UsefulResources");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Link).IsRequired();
            builder.Property(u => u.Description).IsRequired();
            builder.Property(u => u.Advanced).IsRequired();

            builder
                .HasOne(u => u.Lesson)
                .WithMany(l => l.UsefulResources)
                .HasForeignKey(u => u.LessonId);
        }
    }
}

