using System;
using csharp_it.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace csharp_it.Configurations
{
	public class TarifConfiguration : IEntityTypeConfiguration<Tarif>
    {
        public void Configure(EntityTypeBuilder<Tarif> builder)
        {
            builder.ToTable("Tarifs");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.PriceMonth).IsRequired();
            builder.Property(t => t.PriceYear).IsRequired();
            builder.Property(t => t.Description).IsRequired();

            builder
                .HasOne(t => t.Course)
                .WithMany(c => c.Tarifs)
                .HasForeignKey(t => t.CourseId);

            builder
                .HasMany(t => t.UserCourses)
                .WithOne(uc => uc.Tarif)
                .HasForeignKey(uc => uc.TarifId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasMany(t => t.TarifAccesses)
                .WithOne(ta => ta.Tarif)
                .HasForeignKey(ta => ta.TarifId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

