using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class PracticalExampleConfiguration : IEntityTypeConfiguration<PracticalExample>
    {
        public void Configure(EntityTypeBuilder<PracticalExample> builder)
        {
            builder.ToTable("PracticalExamples");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Code).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();

            builder
                .HasOne(p => p.Lesson)
                .WithMany(l => l.PracticalExamples)
                .HasForeignKey(p => p.LessonId);
        }
    }
}

