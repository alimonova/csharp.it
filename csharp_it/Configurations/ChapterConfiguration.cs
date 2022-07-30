using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Number).IsRequired();
            builder.Property(c => c.Name).IsRequired();

            builder
                .HasMany(c => c.Lessons)
                .WithOne(l => l.Chapter)
                .HasForeignKey(l => l.ChapterId);
        }
    }
}

