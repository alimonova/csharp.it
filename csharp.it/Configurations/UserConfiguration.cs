using System;
using csharp.it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp.it.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.Blocked).IsRequired();

            builder
                .HasMany(u => u.UserTasks)
                .WithOne(ut => ut.Student)
                .HasForeignKey(ut => ut.StudentId);

            builder
                .HasMany(u => u.UserTasks)
                .WithOne(ut => ut.Teacher)
                .HasForeignKey(ut => ut.TeacherId);

            builder
                .HasMany(u => u.UserGroups)
                .WithOne(ug => ug.User)
                .HasForeignKey(ug => ug.UserId);

            builder
                .HasMany(u => u.Courses)
                .WithOne(c => c.Author)
                .HasForeignKey(c => c.AuthorId);

            builder
                .HasMany(u => u.Groups)
                .WithOne(g => g.Teacher)
                .HasForeignKey(g => g.TeacherId);
        }
    }
}

