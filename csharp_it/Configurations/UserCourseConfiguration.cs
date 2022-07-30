using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class UserCourseConfiguration : IEntityTypeConfiguration<UserCourse>
    {
        public void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            builder.ToTable("UserCourses");
            builder.HasKey(ug => ug.Id);

            builder.Property(ug => ug.Progress).IsRequired();

            builder
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(ug => ug.UserId);

            builder
                .HasOne(ug => ug.Course)
                .WithMany(g => g.Students)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey(ug => ug.CourseId);

            builder
                .HasOne(ug => ug.Tarif)
                .WithMany(t => t.UserCourses)
                .HasForeignKey(ug => ug.TarifId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

