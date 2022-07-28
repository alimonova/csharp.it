using csharp.it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp.it.Configurations
{
	public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Description).IsRequired();

            builder
                .HasMany(c => c.Chapters)
                .WithOne(ch => ch.Course)
                .HasForeignKey(ch => ch.CourseId);

            builder
                .HasMany(c => c.Tarifs)
                .WithOne(t => t.Course)
                .HasForeignKey(t => t.CourseId);

            builder
                .HasMany(c => c.Students)
                .WithOne(uc => uc.Course)
                .HasForeignKey(uc => uc.CourseId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(c => c.Author)
                .WithMany(a => a.Courses)
                .HasForeignKey(c => c.AuthorId);
        }
    }
}

