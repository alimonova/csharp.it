using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired();
            builder.Property(c => c.Duration).IsRequired();
            builder.Property(c => c.Description).IsRequired();

            builder
                .HasMany(c => c.Chapters)
                .WithOne(ch => ch.Course)
                .HasForeignKey(ch => ch.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(c => c.Tarifs)
                .WithOne(t => t.Course)
                .HasForeignKey(t => t.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

