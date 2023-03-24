using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Number).IsRequired();
            builder.Property(l => l.Topic).IsRequired();
            builder.Property(l => l.ContentLink).IsRequired();
            builder.Property(l => l.VideoLink).IsRequired();

            builder
                .HasMany(l => l.Questions)
                .WithOne(q => q.Lesson)
                .HasForeignKey(q => q.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(l => l.PracticalExamples)
                .WithOne(p => p.Lesson)
                .HasForeignKey(p => p.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(l => l.UsefulResources)
                .WithOne(u => u.Lesson)
                .HasForeignKey(u => u.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(l => l.Tasks)
                .WithOne(t => t.Lesson)
                .HasForeignKey(t => t.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(l => l.Chapter)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.ChapterId);
        }
    }
}

